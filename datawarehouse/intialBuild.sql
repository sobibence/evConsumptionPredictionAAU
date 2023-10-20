CREATE TYPE road_type AS ENUM ('asphalt'); -- lets add these as we are introduced to them by the road data?


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

CREATE TABLE vehicle_trip_status (
	id serial PRIMARY KEY,
	vehicle_model_id int REFERENCES vehicle_model(id),
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
	rain_mm int,
	road_quality int, -- TODO we need to define a range or something here, if it does not exists
	road_type road_type
);

CREATE TABLE node(
	id serial PRIMARY KEY, -- we will have this beside the key from provider to make sure we are able to store data from a different provider in the future
	latitude float8,
	longitude float8,
	longitude_meters int, 
	osm_node_id bigint --provider id this should be 64 bit
);

CREATE TABLE edge(
	id serial PRIMARY KEY, -- we will have this beside the key from provider to make sure we are able to store data from a different provider in the future
	length_meters float,
	allowed_speed_kmph int,
	inclination_degress float,
	start_node_id int REFERENCES node(id),
	end_node_id int REFERENCES node(id),
	average_speed_kmph float,
	osm_way_id bigint --provider id
);

CREATE TABLE fact_estimated_consumption (
	id serial PRIMARY KEY,
	edge_id int REFERENCES edge(id),
	day_in_year smallint,
	minute_in_day smallint,
	vehicle_id int REFERENCES vehicle_model(id),
	weather_id int REFERENCES weather(id),
	energy_consumption_wh float
);

CREATE TABLE fact_recorded_travel(
	speed_km_per_hour float,
	weather_id int REFERENCES weather(id),
	edge_id int REFERENCES edge(id),
	edge_percent float,
	time_epoch timestamp, --this is timestamp, time only records the time of day.
	acceleration_metre_per_second_squared float, -- we might not need this
	energy_consumption_wh float,
	vehicle_id int REFERENCES vehicle_model(id)
);