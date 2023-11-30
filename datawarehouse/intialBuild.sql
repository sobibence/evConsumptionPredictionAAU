CREATE EXTENSION IF NOT EXISTS postgis;
--CREATE EXTENSION IF NOT EXISTS timescaledb;

CREATE TYPE estimation_type AS ENUM ('ml','function_fit','record');

CREATE TYPE surface AS ENUM (
    'paved',
    'asphalt',
    'concrete',
    'concrete:plates',
    'sett',
    'unpaved',
    'compacted',
    'fine_gravel',
    'gravel',
    'pebblestone',
    'ground',
    'dirt',
    'earth',
    'grass',
    'grass_paver',
    'gravel_turf',
    'mud',
    'sand',
    'paving_stones',
    'cobblestone',
    'metal',
    'wood',
    'woodchips',
    'compacted_snow',
    'ice',
    'salt',
    'grasscrete',
    'asphalt;concrete',
    'concrete;grass',
    'concrete;gravel',
    'concrete;asphalt',
    'unpaved;grass',
	'rock',	
	'stone',
	'unhewn_cobblestone',
	'metal_grid',
	'stepping_stones',
	''
);

CREATE TYPE highway AS ENUM (
    'motorway_link',
    'trunk_link',
    'primary_link',
    'secondary_link',
    'tertiary_link',
	'motorway',
    'trunk',
    'primary',
    'secondary',
    'tertiary',
    'unclassified',
    'residential',
    'service',
    'track',
    'path',
    'pedestrian',
    'footway',
    'bridleway',
    'cycleway',
    'steps',
    'living_street',
    'road',
	'platform',
    'construction',
    'bus_guideway',
    'escape',
    'raceway',
    'services',
    'rest_area',
	'proposed',
	'planned',
	''
);



CREATE TABLE producer (
	id serial primary key,
	name varchar(255)
);

CREATE TABLE vehicle_model (
	id serial PRIMARY KEY,
	battery_size_wh int,
	rolling_resistance float,
	drag_coefficient float,
	frontal_size float,
	weight_kg int,
	avg_consumption_wh_km float,
	"name" varchar(255),
	ac_power int,
	"power" int,
	producer_id int REFERENCES producer(id),
	"year" int,
	pt_effeciency float
);

CREATE TABLE vehicle (
	id serial PRIMARY KEY,
	vehicle_model_id int REFERENCES vehicle_model(id)
);

CREATE TABLE vehicle_trip_status (
	id serial PRIMARY KEY,
	vehicle_id int REFERENCES vehicle(id),
	additional_weight_kg int,
	vehicle_milage_meters int,
	finished boolean
	-- driver_aggresiveness int
);

CREATE TABLE weather (
	id serial PRIMARY KEY,
	temperature_celcius float,
	wind_km_ph float,
	wind_direction_degrees smallint, -- 0-360
	fog_percent float,
	sunshine_w_m float,
	rain_mm int
);

CREATE TABLE node(
	id serial PRIMARY KEY, -- we will have this beside the key from provider to make sure we are able to store data from a different provider in the future
	gps_coords geography(Point,4326),
	longitude_meters float8, 
	osm_node_id bigint UNIQUE--provider id this should be 64 bit
);

CREATE TABLE edge_info(
	id serial PRIMARY KEY, -- we will have this beside the key from provider to make sure we are able to store data from a different provider in the future
	speed_limit_kmph int,
	inclination_degress float,
	average_speed_kmph float,
	osm_way_id bigint UNIQUE, --provider id
	street_name varchar(32),
	surface surface,
	highway highway
);

CREATE TABLE edge(
	id serial PRIMARY KEY,
	start_node_id bigint REFERENCES node(osm_node_id),
	end_node_id bigint REFERENCES node(osm_node_id),
	edge_info_id bigint REFERENCES edge_info(osm_way_id)
);

CREATE TABLE fact_estimated_consumption (
	id serial PRIMARY KEY,
	edge_id int REFERENCES edge(id),
	day_in_year smallint,
	minute_in_day smallint,
	vehicle_id int REFERENCES vehicle_model(id),
	weather_id int REFERENCES weather(id),
	estimated_consumption_wh float,
	estimation_type estimation_type
);

CREATE TABLE fact_recorded_travel(
	speed_km_per_hour float,
	weather_id int REFERENCES weather(id),
	edge_id int REFERENCES edge(id),
	trip_id int REFERENCES vehicle_trip_status(id), 
	edge_percent float,
	time_epoch timestamp, --this is timestamp, time only records the time of day.
	acceleration_metre_per_second_squared float, -- we might not need this
	energy_consumption_wh float,
	vehicle_id int REFERENCES vehicle_model(id)
);

CREATE INDEX idx_edge_info_osm_way_id on edge_info(osm_way_id);
CREATE INDEX idx_node_osm_node_id on node(osm_node_id);