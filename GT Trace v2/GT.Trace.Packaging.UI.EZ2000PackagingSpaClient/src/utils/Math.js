/**
 * Determines whether a value n is an integer number or not.
 * ? https://stackoverflow.com/a/14794066
 * @param value The number to be checked.
 */
export const isInt = (value) => {
    return (
        !isNaN(value) &&
        (function (x) {
        return (x | 0) === x;
        })(parseFloat(value))
    );
};