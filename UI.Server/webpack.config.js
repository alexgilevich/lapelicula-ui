const path = require('path');

module.exports = {
    entry: './wwwroot/lib/user-preferences/component.js', // Source directory and entry file
    output: {
        path: path.resolve(__dirname, 'wwwroot/lib/user-preferences/dist'), // Output directory
        filename: 'react-user-preferences.component.js', // Output file name
        libraryTarget: 'umd',
        library: 'ReactUserPreferences',
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
        ],
    },
    resolve: {
        extensions: ['.js', '.jsx'], // Resolve these extensions
    },
    devtool: 'source-map',
    mode: 'development', // Set mode to 'development' or 'production'
};