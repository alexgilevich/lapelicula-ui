const path = require('path');

module.exports = {
    entry: {
        'ReactUserPreferences': './wwwroot/lib/ReactUserPreferences/component.js',
        'MoviesSlideshow': './wwwroot/lib/MoviesSlideshow/component.js',
    }, // Source directory and entry file
    output: {
        path: path.resolve(__dirname, 'wwwroot/lib/'), // Output directory
        filename: '[name]/dist/component.bundle.js', // Output file name
        libraryTarget: 'umd',
        library: '[name]',
        globalObject: 'this'
    },
    module: {
        rules: [
            {
                test: /\.js$/, // Process JavaScript files
                exclude: /node_modules/, // Exclude dependencies
                use: {
                    loader: 'babel-loader', // Transpile ES6+ to ES5
                    options: {
                        presets: [
                            '@babel/preset-env',    // Modern JS features
                            '@babel/preset-react'   // JSX and React transforms
                        ],
                    },

                },
            },
            {
                test: /\.css$/i,
                use: ["style-loader", "css-loader"],
            },
            {
                test: /\.s[ac]ss$/i,
                use: [
                    // Creates `style` nodes from JS strings
                    "style-loader",
                    // Translates CSS into CommonJS
                    "css-loader",
                    // Compiles Sass to CSS
                    "sass-loader",
                ],
            }
            
        ],
    },
    resolve: {
        extensions: ['.js', '.jsx'], // Resolve these extensions
    },
    devtool: 'source-map',
    mode: 'development', // Set mode to 'development' or 'production'
};