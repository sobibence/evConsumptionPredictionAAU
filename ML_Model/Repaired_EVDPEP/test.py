from models import models as models
from utils import losses
import tensorflow as tf
import numpy as np

preloaded_model = None

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

print("test")

load_model()
predictions = preloaded_model.predict(np.random.rand(1,550,30))
print("2")
print(predictions)