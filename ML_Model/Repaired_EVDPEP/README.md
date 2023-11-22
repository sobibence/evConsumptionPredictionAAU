# EVDPEP
[Probabilistic Deep Learning for Electric-Vehicle Energy-Use Prediction](https://dl.acm.org/doi/fullHtml/10.1145/3469830.3470915)


The [VED dataset](https://drive.google.com/drive/folders/1NxGQzGXARK7qCSMHlsuL-Ovrl-0itisL?usp=sharing) used in experiments.


Cite:

    Petkevicius, L., Saltenis, S., Civilis, A., & Torp, K. (2021, August). Probabilistic Deep Learning for Electric-Vehicle Energy-Use Prediction. In 17th International Symposium on Spatial and Temporal Databases (pp. 85-95).
    
or


     @inproceedings{petkevicius2021probabilistic,
      title={Probabilistic Deep Learning for Electric-Vehicle Energy-Use Prediction},
      author={Petkevicius, Linas and Saltenis, Simonas and Civilis, Alminas and Torp, Kristian},
      booktitle={17th International Symposium on Spatial and Temporal Databases},
      pages={85--95},
      year={2021}
    }

# New version
We were not able to run this application trying out different versions of libaries. And so we have done modifications to make it run. To make our version run properly, follow these steps:

1. Install python version 3.9.9 (NOT a newer one) https://www.python.org/downloads/windows/
2. For windows, Add following paths to system environment variable Path (on installation you are asked if this should be done automaticly but did not work for me). Replace paths with location of your python installation
-D:\Users\Malthe\AppData\Local\Programs\Python\Python39
-D:\Users\Malthe\AppData\Local\Programs\Python\Python39\Scripts
3. Open git bash in repository folder and run command 'pip install -r requirements.txt'



to run the endpoint: 
uvicorn endpoint:app --host 0.0.0.0 --port 8000 --reload

dont forget to build the model and replace the path in the endpoint to you .h5 path
build the model command:
python ./main.py --model lstm --datadir al --epochs 400 --lossfunc mse --optimizer adam --batchsize 128 --summed 0 --outputdir output --name final --speedprofile speed_limit