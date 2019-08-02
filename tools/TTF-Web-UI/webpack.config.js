var path = require("path");

module.exports = {
    mode: 'development',
    resolve: {
        extensions: ['.ts', '.tsx', '.js'],
        modules: [
            path.resolve(__dirname, 'node_modules'),
            path.resolve(__dirname, 'src'),
            path.resolve(__dirname, '../TaxonomyObjectModel/out/ts')
        ],
    },
    entry: "./src/index.ts",
    output: {
        filename: "bundle.js",
        path: path.resolve(__dirname, "dist"),
        publicPath: "/dist/"
    },
    module: {
        rules: [
            { test: /\.tsx?$/, loader: "ts-loader" }
        ]
    },
    devServer: {
        stats: {
            assets: false,
            hash: false,
            chunks: false,
            errors: true,
            errorDetails: true,
        },
        overlay: true
    },

};