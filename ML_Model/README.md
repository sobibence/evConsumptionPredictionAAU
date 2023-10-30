Information on how to setup the ML model provided by https://github.com/linas-p/EVDPEP

1. Clone repository https://github.com/linas-p/EVDPEP
2. Install python version 3.11.6 (NOT a newer one)
3. For windows, Add following paths to system environment variable Path (on installation you are asked if this should be done automaticly but did not work for me). Replace paths with location of your python installation
-D:\Users\Malthe\AppData\Local\Programs\Python\Python311
-D:\Users\Malthe\AppData\Local\Programs\Python\Python311\Scripts
4. Open git bash in repository folder and run commands
- pip freeze > requirements.txt
- pip install -r requirements.txt
- pip install tensorflow
- pip install scikit-learn
- pip install focal_loss
- pip install tensorflow_probability
- pip install IPython
- pip install pandas
- pip install matplotlib