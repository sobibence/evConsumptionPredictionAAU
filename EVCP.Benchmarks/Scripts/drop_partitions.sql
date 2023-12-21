DROP TABLE IF EXISTS fact_estimated_consumption;

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