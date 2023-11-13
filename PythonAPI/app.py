from flask import Flask, request, jsonify
from keras.models import load_model
import sys

model = load_model('ML_Model.h5')
model.summary()

app = Flask(__name__)

@app.route('/consumption/', methods=['GET'])

def consumption():
    # This should be of type modelInput, but there is no typing of variables in python
    print(request.get_json())
    input = request.get_json()
    return str(input['meters'])

if __name__ == '__main__':
    app.run(debug=True)