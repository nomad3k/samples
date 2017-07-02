module.exports = {
  "parser": "babel-eslint",
  "plugins": [
  ],
  "parserOptions": {
    "ecmaVersion": 6,
    "sourceType": "module",
    "ecmaFeatures": {
      "jsx": true
    }
  },
  "env": {
    "browser": true,
    "amd": true,
    "es6": true,
    "node": true,
    "mocha": true
  },
  "extends": "eslint:recommended",
  "parserOptions": {
    "sourceType": "module"
  },
  "rules": {
    "no-var": "error",
    "comma-dangle": "error",
    "quotes": [ "warn", "single" ],
    "no-undef": "error",
    "no-extra-semi": "error",
    "no-underscore-dangle": "warn",
    "no-console": "error",
    "no-trailing-spaces": ["error", { "skipBlankLines": true }],
    "no-unreachable": "error",
    "no-unused-vars": ["warn", { "argsIgnorePattern": "^_" }],
    "no-alert": "warn",
    "global-strict": 0
  }
};
