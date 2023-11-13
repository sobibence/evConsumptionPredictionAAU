from fastapi import FastAPI, HTTPException, Depends

from models import models as models
from utils import losses
import tensorflow as tf
import numpy as np

preloaded_model = None

app = FastAPI()

@app.on_event("startup")
def load_model():
    print("Startup")
    global preloaded_model 
    preloaded_model = tf.keras.models.load_model('output/v3_dnn_mse_al_speed_limit_1_adam_epoch=40/v3_last-only_epoch=40.h5', custom_objects={'gaussian_nll': losses.gaussian_nll})
    input_layer = preloaded_model.layers[0]  # Assuming the input layer is the first layer
    input_shape = input_layer.input_shape
    # input_data_type = input_layer.input_dtype

    print("Input Shape:", input_shape)
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
        print("1")
        # Make predictions
        #predictions = model.predict(input_data)
        predictions = model.predict({
             "feature_1": np.random.rand(100),
            "feature_2": np.random.rand(100),
            }, verbose=0)[:10]
        
        print("2")
        return {"predictions": predictions.tolist()}

    except Exception as e:
        raise HTTPException(status_code=500, detail="Prediction error")

def preprocess_data(data):
    array=np.zeros(30)
    return array 