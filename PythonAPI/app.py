from flask import Flask, request, jsonify
import sys

app = Flask(__name__)

@app.route('/consumption/', methods=['GET'])

def consumption():
    # This should be of type modelInput, but there is no typing of variables in python
    print(request.get_json())
    input = request.get_json()
    return str(input['meters'])

if __name__ == '__main__':
    app.run(debug=True)