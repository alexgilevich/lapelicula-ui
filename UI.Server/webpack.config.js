const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const webpack = require('webpack');

module.exports = {
    entry: {
        'app-react': './wwwroot/app-react/index.js',
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot/dist/'), // Output directory
        filename: '[name].bundle.js', // Output file name
        clean: true
    },
    module: {
        rules: [
            {
                test: /\.jsx?$/, // Process JavaScript files
                exclude: /node_modules/, // Exclude dependencies
                use: {
                    loader: 'babel-loader', // Transpile ES6+ to ES5
                    options: {
                        presets: [
                            '@babel/preset-env',    // Modern JS features
                            ['@babel/preset-react', { runtime: 'automatic' }]   // JSX and React transforms
                        ],
                    },

                },
            },
            {
                test: /\.css$/i,
                use: [MiniCssExtractPlugin.loader, "css-loader"],
            },
            {
                test: /\.s[ac]ss$/i,
                use: [
                    // Extracts styles into a separate CSS file
                    MiniCssExtractPlugin.loader,
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
    plugins: [
        new MiniCssExtractPlugin({
            filename: '[name].css' // You can also use '[name].[contenthash].css' for caching
        })
    ],
    devtool: 'source-map',
};