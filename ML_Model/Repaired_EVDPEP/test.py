from models import models as models
from utils import losses
import tensorflow as tf
import numpy as np
import json

preloaded_model = None

jsonstr = """{
 "driving_data": [    [0.5, 0.3, 0.1, 0.2, 0.9,0.5, 0.3, 0.1, 0.2, 0.9,0.5, 0.3, 0.1, 0.2, 0.9,0.5, 0.3, 0.1, 0.2, 0.9,0.5, 0.3, 0.1, 0.2, 0.9,0.5, 0.3, 0.1, 0.2, 0.9],     [0.2, 0.7, 0.4, 0.1, 0.2, 0.6, 0.5, 0.3, 0.1, 0.2, 0.9,0.5, 0.3, 0.1, 0.2, 0.9,0.5, 0.3, 0.1, 0.2, 0.9,0.5, 0.3, 0.1, 0.2, 0.9,0.5, 0.3, 0.1, 0.9]   ]}
 """

def load_model():
    print("Startup")
    global preloaded_model 
    preloaded_model = tf.keras.models.load_model('/home/sobibence/project/evConsumptionPredictionAAU/ML_Model/Repaired_EVDPEP/output/final_lstm_mse_al_speed_limit_0_adam_epoch=400/final_last-only_epoch=400.h5', custom_objects={'gaussian_nll': losses.gaussian_nll})
    # input_layer = preloaded_model.layers[0]  # Assuming the input layer is the first layer
    # input_shape = input_layer.input_shape
    # # input_data_type = input_layer.input_dtype

    # print("Input Shape:", input_shape)
    # # print("Input Data Type:", input_data_type)
    # print(preloaded_model.summary())

print("test")

load_model()
json_data = json.loads(jsonstr)
matrix = json_data['driving_data']
matrix_np = np.array(matrix)
matrix_np_T = matrix_np.T

# print("T shape")
# print(matrix_np_T.shape)
print("normal")
print(matrix_np.shape)
#padded = np.pad(matrix_np, ((0, 550-matrix_np.shape[0]), (0, 30-matrix_np.shape[1])), 'constant', constant_values=-1)
padded = matrix_np
print(padded.shape)

print (padded)
# shape = (1,550,30)
# values = np.full(shape, 0)
# print("response ")
reshaped = np.expand_dims(padded, axis =0)
print(reshaped.shape)
# print(reshaped)

# print(matrix_np.shape)
# print(reshaped)
predictions = preloaded_model.predict(reshaped)
print(predictions)