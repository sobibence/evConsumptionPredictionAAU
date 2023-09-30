

CREATE TYPE battery_size_type AS ENUM ('small', 'medium', 'large');
CREATE TYPE road_quality_type AS ENUM ('bad', 'medium', 'good');

CREATE TABLE vehicle_model (
	id serial PRIMARY KEY,
	battery_size battery_size_type,
	rolling_resistance int,
	drag_coefficient int,
	frontal_size int,
	weight_gram int,
	avg_consumption_wh_km int,
	"name" varchar(255),
	ac_power int,
	"power" int,
	producer_name varchar(255), -- should this not be a foreign key to a producer?
	"year" int,
	pt_effeciency int
);

CREATE TABLE vehicle (
	id serial PRIMARY KEY,
	vehicle_model_id int REFERENCES vehicle_model(id)
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
	road_quality road_quality_type
	-- road_type road_type
);

CREATE TABLE consumption (
	id serial PRIMARY KEY,
	-- edge_id int, Why here?
	day_in_year smallint,
	minute_in_day smallint,
	-- vehicle_id int REFERENCES vehicle(id) WHY?
	-- weather_id int REFERENCES weather(id) WHY?
	energy_use_wh float
);

CREATE TABLE timestamp (
	id serial PRIMARY KEY,
	epoch_seconds int
);

CREATE TABLE fact_travel(
	speed_km_per_hour float,
	weather_id int REFERENCES weather(id),
	edge_id int,
	-- edge_percent float, What is it
	timestamp_id int REFERENCES timestamp(id),
	acceleration_metre_per_second_squared float,
	energy_consumption_Kwh float,
	vehicle_id int REFERENCES vehicle(id)
);

