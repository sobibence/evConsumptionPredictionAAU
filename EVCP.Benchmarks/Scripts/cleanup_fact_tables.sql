DELETE FROM fact_recorded_travel;
TRUNCATE TABLE fact_estimated_consumption RESTART IDENTITY; 
DELETE FROM weather;
ALTER SEQUENCE weather_id_seq RESTART WITH 1;