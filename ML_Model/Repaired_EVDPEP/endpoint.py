from fastapi import FastAPI, HTTPException, Depends

from models import models as models
from utils import losses
import tensorflow as tf
import numpy as np
import json
import traceback

preloaded_model = None

app = FastAPI()

@app.on_event("startup")
def load_model():
    print("Startup")
    global preloaded_model 
    preloaded_model = tf.keras.models.load_model('/home/sobibence/project/evConsumptionPredictionAAU/ML_Model/Repaired_EVDPEP/output/final_lstm_mse_al_speed_avg_0_adam_epoch=4000/final_last-only_epoch=4000.h5', custom_objects={'gaussian_nll': losses.gaussian_nll})
    # input_layer = preloaded_model.layers[0]  # Assuming the input layer is the first layer
    # input_shape = input_layer.input_shape
    # # input_data_type = input_layer.input_dtype

    # print("Input Shape:", input_shape)
    # print("Input Data Type:", input_data_type)
    print(preloaded_model.summary())


def get_model():
    if preloaded_model is None:
        print("module not loaded")
        raise HTTPException(status_code=500, detail="Model not loaded")
        
    return preloaded_model


@app.post("/predict/")
async def predict(data: dict, model: tf.keras.Model = Depends(get_model)):
    try:
        #print( model.summary())
        input_data = preprocess_data(data)
        # print("1")
        # Make predictions
        predictions = model.predict(input_data).flatten()
        print(predictions)
        return predictions.tolist()

    except Exception as e:
        print(Exception, e)
        print(traceback.format_exc())
        raise HTTPException(status_code=500, detail="Prediction error")


def preprocess_data(data):
    #jsonData = json.load(data)
    print(data)
    matrix = data['driving_data']
    matrix_np = np.array(matrix)
    reshaped = np.expand_dims(matrix_np, axis =0)
    print(reshaped)
    return reshaped