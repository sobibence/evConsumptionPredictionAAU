DROP TABLE IF EXISTS fact_estimated_consumption;

CREATE TABLE IF NOT EXISTS fact_estimated_consumption (
	id serial,
	edge_id int REFERENCES edge(id),
	day_in_year smallint,
	minute_in_day smallint,
	vehicle_id int REFERENCES vehicle_model(id),
	weather_id int REFERENCES weather(id),
	estimated_consumption_wh float,
	estimation_type estimation_type,
	CONSTRAINT fact_estimated_consumption_pkey PRIMARY KEY (id, vehicle_id)
) PARTITION BY LIST (vehicle_id);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_1 
PARTITION OF fact_estimated_consumption
    FOR VALUES IN (1);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_2 
PARTITION OF fact_estimated_consumption
    FOR VALUES IN (2);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_3 
PARTITION OF fact_estimated_consumption
    FOR VALUES IN (3);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_4 
PARTITION OF fact_estimated_consumption
    FOR VALUES IN (4);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_5 
PARTITION OF fact_estimated_consumption
    FOR VALUES IN (5);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_6 
PARTITION OF fact_estimated_consumption
    FOR VALUES IN (6);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_7 
PARTITION OF fact_estimated_consumption
	FOR VALUES IN (7);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_8 
PARTITION OF fact_estimated_consumption
    FOR VALUES IN (8);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_9 
PARTITION OF fact_estimated_consumption
    FOR VALUES IN (9);

CREATE TABLE IF NOT EXISTS fact_estimated_consumption_10 
PARTITION OF fact_estimated_consumption
    FOR VALUES IN (10);
    