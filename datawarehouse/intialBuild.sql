

CREATE TYPE road_type AS ENUM ('asphalt'); -- lets add these as we are introduced to them by the road data?

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

CREATE TABLE producer (
	id serial primary key,
	name varchar(255)
)

CREATE TABLE vehicle_trip_status (
	id serial PRIMARY KEY,
	vehicle_model_id int REFERENCES vehicle_model(id),
	additional_weight_grams int,
	vehicle_milage_meters int,
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

CREATE TABLE fact_consumption (
	id serial PRIMARY KEY,
	edge_id int,
	day_in_year smallint,
	minute_in_day smallint,
	vehicle_id int REFERENCES vehicle(id),
	weather_id int REFERENCES weather(id),
	energy_use_wh float
);

CREATE TABLE fact_travel(
	speed_km_per_hour float,
	weather_id int REFERENCES weather(id),
	edge_id int,
	edge_percent float,
	time_epoch time,
	acceleration_metre_per_second_squared float,
	energy_consumption_Kwh float,
	vehicle_id int REFERENCES vehicle(id)
);

