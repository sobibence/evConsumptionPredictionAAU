import numpy as np
import sklearn as sklearn 
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LinearRegression
from sklearn import metrics

# Generate synthetic data for energy consumption
np.random.seed(42)

temperature = np.random.uniform(0, 40, 100)
minutes_in_day = np.random.randint(0, 1440, 100)
rolling_resistance = np.random.uniform(0.1, 0.5, 100)
rolling_coefficient = np.random.uniform(0.01, 0.05, 100)

energy_consumption = (
    15 + 
    0.7 * temperature - 
    0.05 * minutes_in_day + 
    1.5 * rolling_resistance + 
    2 * rolling_coefficient + 
    np.random.randn(100)
)

# Combine independent variables into a feature matrix
X = np.column_stack((temperature, minutes_in_day, rolling_resistance, rolling_coefficient))

# Split the data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(X, energy_consumption, test_size=0.2, random_state=42)

# Create a linear regression model
model = LinearRegression()

# Fit the model to the training data
model.fit(X_train, y_train)

# Make predictions on the test set
y_pred = model.predict(X_test)

# Evaluate the model
print('Mean Absolute Error:', metrics.mean_absolute_error(y_test, y_pred))
print('Mean Squared Error:', metrics.mean_squared_error(y_test, y_pred))
print('Root Mean Squared Error:', np.sqrt(metrics.mean_squared_error(y_test, y_pred)))
print('R-squared:', metrics.r2_score(y_test, y_pred))

# Coefficients and intercept
print('Coefficients:', model.coef_)
print('Intercept:', model.intercept_)