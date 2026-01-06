import os

from mlflow.pyfunc import PyFuncModel

from features import UserPreferences

# This module is just a facade for model inference only. The model is loaded automatically using Mlflow from a specified location.
# The module is used to generate C# bindings automatically using CSnake source code generator

model: PyFuncModel = None

import mlflow
import numpy as np

def load_model():
    global model
    model_save_bucket = os.environ.get("MODEL_SAVE_S3_BUCKET")
    model_save_prefix = os.environ.get("MODEL_SAVE_S3_PREFIX")
    model_name = os.environ.get("MLFLOW_MODEL_NAME")
    model = mlflow.pyfunc.load_model(f"s3://{os.path.join(model_save_bucket, model_save_prefix, model_name)}")


def recommend(preferences: dict[str, float], movie_vectors: bytes) -> list[tuple[int, float]]:
    """
    Recommend items based on the given user preferences.

    This function takes a dictionary of user preferences as input, converts 
    it into an appropriate format, and generates recommendations using a 
    pre-trained model. The recommendations are returned as a list of tuples,
    where each tuple consists of movie id and its predicted rating.

    :param movie_vectors: One-hot encoded movie vectors represented as a byte array. The shape of the array is: (number of movies, number of genres + 1). 
        Example: [13,0,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0] where 13 is the movie ID.
    :param preferences: A dictionary representing user preferences with item 
        keys and their respective preference scores/ratings as values.
    :return: A list of tuples, where each tuple contains an item (str) and 
        its predicted score (float).
    """
    if not is_loaded():
        raise ValueError("Model has not been loaded yet")
    
    assert movie_vectors, "`movie_vectors` argument is required to be provided"
    assert preferences, "`preferences` argument is required to be provided" 
    
    if __debug__:
        print("Predicting for preferences: ", preferences)

    preferences = UserPreferences(**preferences).to_dict()
    if __debug__:
        print("All preferences: ", preferences)
    
    movie_vectors_np = np.frombuffer(movie_vectors, dtype=np.int32).reshape(-1, len(preferences.items()) + 1)
    res = model.predict([
        {
            "user_preferences": preferences,
            "movies": movie_vectors_np
        }
    ])[0]
    return res

def is_loaded() -> bool:
    """
    Determines whether the model has been loaded using MLFlow.

    :return: A boolean indicating whether the model has been loaded.
    """
    return bool(model)

load_model()

if __name__ == '__main__':
    movies = np.array([[1,0,1,1,1,0,0,0,1,0,0,1,0,0,0,0,0,0,0],
                       [2,0,1,0,0,0,0,0,1,0,0,1,0,0,0,0,0,0,0],
                       [3,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0],
                       [4,0,0,0,1,0,0,1,0,0,0,0,0,0,1,0,0,0,0],
                       [5,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
                       [6,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0],
                       [7,0,0,0,1,0,0,0,0,0,0,0,0,0,1,0,0,0,0],
                       [8,0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0],
                       [9,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
                       [10,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0],
                       [11,0,0,0,1,0,0,1,0,0,0,0,0,0,1,0,0,0,0],
                       [12,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0],
                       [13,0,1,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0],
                       [14,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0],
                       [15,1,1,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0],
                       [16,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0,0,0,0],
                       [17,0,0,0,0,0,0,1,0,0,0,0,0,0,1,0,0,0,0],
                       [18,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
                       [19,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
                       [20,1,0,0,1,1,0,1,0,0,0,0,0,0,0,0,1,0,0]], dtype=np.int32)
    requests = [{
        "user_preferences": { "drama": 5, "action": 5 },
        "movies": movies
    }]

    
    def recommend_internal(movie_ids: list[int] = None, **preferences: float) -> list[tuple[str, float]]:
        movies_bytes = memoryview(movies).tobytes()
        return recommend(preferences, movies_bytes)
    
    print('\n\nfirst user (should be more action and adventure movies):')
    predictions = recommend_internal(action = 5, adventure = 3.5, mystery = 4, horror = 1, sci_fi = 4, western = 3, drama = 3, animation = 0.5 )
    for pred in predictions:
        print(pred)

    print('\n\nsecond user (should be more kids-oriented movies and cartoons):')
    predictions = recommend_internal(kids = 5, animation = 5, adventure = 4.5, comedy = 4.5, mystery = 2, crime = 1, horror = 0.5, sci_fi = 4)
    for pred in predictions:
        print(pred)

    print('\n\nthird user (should be more romance-oriented movies):')
    predictions = recommend_internal( comedy = 4.5, romance = 5, mystery = 2, crime = 0.5, horror = 0.5, sci_fi = 1.5)
    for pred in predictions:
        print(pred)

    print('\n\nfourth user (should be only kids-oriented movies and cartoons):')
    predictions = recommend_internal(kids = 5, animation = 5, adventure = 4.5)
    for pred in predictions:
        print(pred)


    print('\n\nfifth user (should be more action and sci-fi movies):')
    predictions = recommend_internal(action = 5,  sci_fi = 4.5 )
    for pred in predictions:
        print(pred)

    print('\n\nsixth user (should be more comedy and romance movies):')
    predictions = recommend_internal(comedy = 4.5,  romance = 4.5 )
    for pred in predictions:
        print(pred)

    print('\n\nseventh user (should be more war-related movies):')
    predictions = recommend_internal(war=5)
    for pred in predictions:
        print(pred)

    print('\n\neighth user (should be more western movies):')
    predictions = recommend_internal(western=5)
    for pred in predictions:
        print(pred)
    