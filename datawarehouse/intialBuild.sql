CREATE TYPE road_type AS ENUM ('asphalt'); -- lets add these as we are introduced to them by the road data?

CREATE TABLE producer (
	id serial primary key,
	name varchar(255)
);

CREATE TABLE vehicle_model (
	id serial PRIMARY KEY,
	battery_size_kwh int,
	rolling_resistance int,
	drag_coefficient float,
	frontal_size float,
	weight_kilogram int,
	avg_consumption_wh_km int,
	"name" varchar(255),
	ac_power int,
	"power" int,
	producer_id int REFERENCES producer(id),
	"year" int,
	pt_effeciency int
);

CREATE TABLE vehicle_trip_status (
	id serial PRIMARY KEY,
	vehicle_model_id int REFERENCES vehicle_model(id),
	additional_weight_grams int,
	vehicle_milage_meters int
	-- driver_aggresiveness int
);

CREATE TABLE weather (
	id serial PRIMARY KEY,
	temperature_celcius float,
	wind_km_ph float,
	wind_direction varchar(2),
	fog_percent float,
	sunshine_w_m float,
	rain_mm int,
	road_quality int, -- TODO we need to define a range or something here, if it does not exists
	road_type road_type
);

CREATE SCHEMA map;
SET search_path = map;

CREATE TABLE node(
	id serial PRIMARY KEY, -- we will have this beside the key from provider to make sure we are able to store data from a different provider in the future
	latitude float,
	longitude float,
	longitude_meters int,
	osm_node_id int --provider id
);

CREATE TABLE edge(
	id serial PRIMARY KEY, -- we will have this beside the key from provider to make sure we are able to store data from a different provider in the future
	length_meters float,
	allowed_speed_kmph int,
	inclination_degress int,
	start_node_id int REFERENCES node(id),
	end_node_id int REFERENCES node(id),
	average_speed_kmph float,
	osm_way_id int --provider id
);

SET search_path = public;

CREATE TABLE fact_consumption (
	id serial PRIMARY KEY,
	edge_id int REFERENCES edge(id),
	day_in_year smallint,
	minute_in_day smallint,
	vehicle_id int REFERENCES vehicle(id),
	weather_id int REFERENCES weather(id),
	energy_use_wh float
);

CREATE TABLE fact_travel(
	speed_km_per_hour float,
	weather_id int REFERENCES weather(id),
	edge_id int REFERENCES edge(id),
	edge_percent float,
	time_epoch time,
	acceleration_metre_per_second_squared float,
	energy_consumption_Kwh float,
	vehicle_id int REFERENCES vehicle(id)
);