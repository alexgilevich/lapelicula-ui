
<h1 align="center">
  <br>
  <a href="https://lapelicula.net/"><img width="200" height="200" alt="watching-a-movie" src="https://github.com/user-attachments/assets/827d93cb-6ea6-4723-9801-a71aaba12432" /></a>
  <br>
  Project La Pelicula
  <br>
</h1>


<h4 align="center">Movie recommender system on top of Tensorflow, Python and .NET</h4>

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
  This is the web UI repository. ML code is included as a git submodule. For ML code check out <a href="https://github.com/alexgilevich/lapelicula-ml" target="_blank">this repo</a>.
</p>

<p align="center">
<img src="https://github.com/user-attachments/assets/d93d183a-b1ff-4bfa-b47b-f94a296ffc91"
         alt="Screenshot">
</p>

## Architecture

From the very inception, the architecture of La Pelicula was conceived to be simple in nature because the purpose of this project is to (a) test out various ideas, (b) explore the limits of the technology and (c) demonstrate how you could train a full-fledged ML model and run it locally in a self-contained autonomous way using C# and Python working hand-in-hand together to show their strong sides: C# and .NET being a convenient platform to run efficient web apps, and Python being the most widespread language for building ML models and running experiments with it. 

For example, the decision to take MovieLens 100k dataset for training initially and not anything bigger than that was made with the goal to keep the resource consumption to minimum and because I believe it should be possible to get a high-quality model for this simple use case (yet!) on the basis of a limitted data set like this. 

Another example is Pandas. Even though I am a certified Databricks Champion, I believe that for now using Spark would be an overkill. Because let's be honest – 100k rating rows is not much of the data in the first place (even though I oversample those to about 500k during [preprocessing](https://github.com/alexgilevich/lapelicula-ml))! Now, I am not saying that I will never use it in the scope of this project. Anything is possible. And, if the current technology presents an obvious limitation, that's about time to scale it out to be more distributed (and probably convenient), right?

Thus, many architectural decisions that have been made and will be made in the future are steming from this initial conception: test out at a small scale first and only then scale it out, if it shows to be benifical. 

### Features:
* Neural Network with two tower architecture (content-based recommender)
* Trained with the (oversampled) [MovieLens 100k](https://grouplens.org/datasets/movielens/) tiny dataset (check out more info in the [ML repo](https://github.com/alexgilevich/lapelicula-ml))
* Python code emdedded and run directly in the .NET process with [CSnakes](https://tonybaloney.github.io/CSnakes/)
* Local training and raw data preprocessing without external dependencies (with Pandas and TensorFlow)
* No external database dependencies
* [TMDB API](https://developer.themoviedb.org/docs/getting-started) integration for getting additional movie attributes
* Stack: .NET, Python, TensorFlow, mix of Blazor/React.js/WebAssembly on the frontend
* Content features: only genres for now (see todo)
* Initial server-side pre-rendering with top 200 movies for SEO optimization purposes
* Predictions on the whole movie data set (~9700 movies currently)

<img width="591" height="1205" alt="recommender-Page-2 drawio" src="https://github.com/user-attachments/assets/045203e1-0f5f-4fd6-9a7e-c4e7bea44b52" />


## How To Build Locally

To run the application locally without Docker, you'll need [.NET SDK](https://dotnet.microsoft.com/en-us/download), [Node.js + npm](https://nodejs.org/en/download/) installed on your computer. Python will be fetched automatically by CSnakes. 
You will also need a TMDB API key which you can get [here](https://developer.themoviedb.org/docs/getting-started).
From your command line:

```bash
# Clone this repository
$ git clone --recurse-submodules https://github.com/alexgilevich/lapelicula-ui

# Go into the repository
$ cd lapelicula-ui

# Set API key
$ export TMDB_API_KEY="{put your TMDB API key here}"

# Run the app
$ dotnet run --project UI.Server/UI.Server.csproj
```

Or with Docker:

```bash
# Clone this repository
$ git clone --recurse-submodules https://github.com/alexgilevich/lapelicula-ui

# Set 
$ cd lapelicula-ui

# Build the image
$ docker build -f UI.Server/Dockerfile -t lapelicula-ui ./

# Set API key
$ echo "TMDB_API_KEY='{put your TMDB API key here}'" > UI.Server/.env

# Run the image
$ docker run --rm -it --mount='type=volume,src=lapelicula-ml-data,dst=/app/ml/data,volume-driver=local' --mount='type=volume,src=lapelicula-ml-artifacts,dst=/app/ml/artifacts,volume-driver=local' -p 8080:8080 -p 8081:8081 --env-file ./UI.Server/.env lapelicula-ui
```

Once the initial training is done, you'll be able to run the code with the trained model saved locally.


## TODO

- Increase the number of movies and add candidate selection phase via ANN or other vector search algorithms
- Add a separate page with all recommended movies
- Migrate from Blazor to React.js on the frontend side (as Blazor is not used much anyway for now)
- everything else in [the ML todo list](https://github.com/alexgilevich/lapelicula-ml)


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

MIT

---

[lapelicula.net](https://lapelicula.net/)


