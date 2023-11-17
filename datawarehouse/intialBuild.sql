CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS timescaledb;

CREATE TYPE estimation_type AS ENUM ('ml','function_fit','record');

CREATE TYPE osm_surface_type AS ENUM (
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
    'unpaved;grass'
);

CREATE TYPE osm_highway_type AS ENUM (
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
    'construction',
    'bus_guideway',
    'escape',
    'raceway',
    'services',
    'rest_area'
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
	vehicle_milage_meters int
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
	latitude float8,
	longitude float8,
	longitude_meters float8, 
	osm_node_id bigint --provider id this should be 64 bit
);

CREATE TABLE edge_info(
	id serial PRIMARY KEY, -- we will have this beside the key from provider to make sure we are able to store data from a different provider in the future
	speed_limit_kmph int,
	inclination_degress float,
	average_speed_kmph float,
	osm_way_id bigint, --provider id
	street_name varchar(32),
	surface osm_surface_type,
	highway osm_highway_type
);

CREATE TABLE edge(
	id serial PRIMARY KEY,
	start_node_id int REFERENCES node(id),
	end_node_id int REFERENCES node(id),
	edge_into_id int REFERENCES edge_info(id)
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
	trip_id int, 
	edge_percent float,
	time_epoch timestamp, --this is timestamp, time only records the time of day.
	acceleration_metre_per_second_squared float, -- we might not need this
	energy_consumption_wh float,
	vehicle_id int REFERENCES vehicle_model(id)
);