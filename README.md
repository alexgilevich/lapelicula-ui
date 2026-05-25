
<h1 align="center">
  <br>
  <a href="https://lapelicula.net/"><img width="200" height="200" alt="watching-a-movie" src="https://github.com/user-attachments/assets/827d93cb-6ea6-4723-9801-a71aaba12432" /></a>
  <br>
  Project La Pelicula
  <br>
</h1>


<h4 align="center">Movie recommender on top of Tensorflow, Python and .NET</h4>

<p align="center">
  <a href="https://github.com/alexgilevich/lapelicula-ui/actions/workflows/aws.yml">
    <img src="https://github.com/alexgilevich/lapelicula-ui/actions/workflows/aws.yml/badge.svg"
         alt="CI/CD">
  </a>
</p>

<p align="center">
  <a href="https://lapelicula.net/">Try it out</a> •
  <a href="#architecture">Architecture</a> •
  <a href="#how-to-build-locally">How To Build Locally</a> •
  <a href="#credits">Credits</a> •
  <a href="#attribution">Attribution</a> •
  <a href="#license">License</a> •
  <a href="#support">Support me</a>
</p>

<p align="center">
  This is the web UI repository. For model training and data preprocessing code, check out <a href="https://github.com/alexgilevich/lapelicula-ml" target="_blank">this repo</a>.
</p>

<p align="center">
<img src="https://github.com/user-attachments/assets/d93d183a-b1ff-4bfa-b47b-f94a296ffc91"
         alt="Screenshot">
</p>

## Architecture

The project consists of two repos: ML and UI. 

ML repo includes code for data preprocessing and model training on top of Apache Spark, MLFlow and TensorFlow. Once trained, the model binaries are saved to S3 as a MLFlow wrapper over TensorFlow Keras model. 

UI repo includes C# / Python backend and React.js frontend code. The backend loads the model from the configured S3 prefix via MLFlow. The code for model loading and inferencing is written entirely with Python. Python and C# code are executed in the same process without any extra Python APIs in between. .NET passes the movie matrix for inferencing to Python via Python Buffer protocol which ensures minimal in-memory data copying. 

The movies list is stored in a Amazon DynamoDB table prefilled by the data preprocessing job (see the ML repo).

Even though Apache Spark code operates with data using Unity Catalog and Delta Lake, UI repo does not depend on them and the only requirement for it to function is to load the trained model and list of all movies.

### Features:
* Neural Network with two tower architecture (content-based recommender)
* Trained with the (oversampled) [MovieLens 100k](https://grouplens.org/datasets/movielens/) tiny dataset (check out more info in the [ML repo](https://github.com/alexgilevich/lapelicula-ml))
* Python code emdedded and run directly in the .NET process with [CSnakes](https://tonybaloney.github.io/CSnakes/)
* Training and raw data preprocessing with Apache Spark, Unity Catalog, Delta Lake, MLFlow and TensorFlow
* [TMDB API](https://developer.themoviedb.org/docs/getting-started) integration for getting additional movie attributes
* Stack: .NET, Python, TensorFlow, React.js, MLFlow, Apache Spark, Unity Catalog
* Content features: only genres for now (see todo)
* Initial server-side pre-rendering with top 200 movies for SEO optimization purposes
* Predictions on the whole movie data set (~9700 movies currently)

<img width="531" height="455" alt="La Pelicula Architecture Diagram" src="https://github.com/user-attachments/assets/fa0ed504-6629-446a-a8ff-9b337e44027b" />


## How To Build Locally

To run the application locally without Docker, you'll need [.NET SDK](https://dotnet.microsoft.com/en-us/download), [Node.js + npm](https://nodejs.org/en/download/) installed on your computer. Python runtime is going to be donwloaded automatically during initialization. 

Prerequisites:

* The DynamoDB table with movies is prefilled with the preprocessing code (either run the preprocessing job in Databricks or run the code locally)
* The model is trained and saved to the configured S3 location with the training code (either run the code in Databricks or run the code locally)


From your command line:

```bash
# Clone this repository
$ git clone https://github.com/alexgilevich/lapelicula-ui

# Go into the repository
$ cd lapelicula-ui

# Set all environment variables or create a .env file
$ export AWS_ACCESS_KEY_ID={your AWS credentials to load the model from S3 and access DynamoDB movie table}
$ export AWS_SECRET_ACCESS_KEY={your AWS credentials to load the model from S3 and access DynamoDB movie table}
$ export AWS_REGION={your AWS credentials to load the model from S3 and access DynamoDB movie table}
$ export MODEL_SAVE_S3_BUCKET=lapelicula
$ export MODEL_SAVE_S3_PREFIX=models/
$ export MLFLOW_MODEL_NAME=lapelicula-movie-recommender-model-new
$ export ASPNETCORE_ENVIRONMENT=Development


# Run the app
$ dotnet run --project UI.Server/UI.Server.csproj
```

Or with Docker:

```bash
# Clone this repository
$ git clone https://github.com/alexgilevich/lapelicula-ui

# Set 
$ cd lapelicula-ui

# Build the image
$  docker build -t lapelicula-ui . 

# create a .env file with the following variables:
# Set all environment variables or create a .env file
# AWS_ACCESS_KEY_ID={your AWS credentials to load the model from S3 and access DynamoDB movie table}
# AWS_SECRET_ACCESS_KEY={your AWS credentials to load the model from S3 and access DynamoDB movie table}
# AWS_REGION={your AWS credentials to load the model from S3 and access DynamoDB movie table}
# MODEL_SAVE_S3_BUCKET=lapelicula
# MODEL_SAVE_S3_PREFIX=models/
# MLFLOW_MODEL_NAME=lapelicula-movie-recommender-model-new
# ASPNETCORE_ENVIRONMENT=Docker


# Run the image
$ docker run --name lapelicula-ui --env-file .env --rm -p 8080:8080 -p 8081:8081 lapelicula-ui      

```



## TODO

- [ ] Increase the number of movies and add candidate selection phase via ANN or other vector search algorithms
- [ ] Add a separate page with all recommended movies
- [x] Migrate from Blazor to React.js on the frontend side (as Blazor is not used much anyway for now)
- [ ] Add user profiles


Feel free to help me with the list above by contributing to this repo :)

## Attribution

- [KMedoids – k-means algorithm alternative](https://github.com/kno10/python-kmedoids)
- [MovieLens dataset](https://grouplens.org/datasets/movielens/)


## Related

[Try it out yourself](https://lapelicula.net/)

## Credits

[Alexandr Gilevich](https://github.com/alexgilevich) – author and main contributor

## Support

If you like this project and think it has helped in any way, consider buying me a coffee!

<a href="https://www.buymeacoffee.com/alexgilevich" target="_blank" b-uzeyq7dyx3=""><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;" b-uzeyq7dyx3=""></a>

## License

* UI and the code in the UI repo is licensed under [GNU General Public License v3.0](LICENSE)
* ML repo is licensed under [MIT](https://github.com/alexgilevich/lapelicula-ml/LICENSE)

---

[lapelicula.net](https://lapelicula.net/)


