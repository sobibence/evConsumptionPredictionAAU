from flask import Flask

app = Flask(__name__)

@app.route('/consumption/<meters>', methods=['GET'])

def consumption():
    return "{meters}"

if __name__ == '__main__':
    app.run(debug=True)