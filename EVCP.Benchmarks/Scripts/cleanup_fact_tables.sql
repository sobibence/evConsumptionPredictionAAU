DELETE FROM fact_recorded_travel;
--TRUNCATE TABLE fact_estimated_consumption RESTART IDENTITY; 
DELETE FROM fact_estimated_consumption;
ALTER SEQUENCE fact_estimated_consumption_id_seq RESTART WITH 1;
DELETE FROM weather;
ALTER SEQUENCE weather_id_seq RESTART WITH 1;