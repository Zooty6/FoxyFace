# API Logic

Each endpoint returns a json structur.
And whenever an endpoint returns an error it'll return additionally an error object:
error: 
    string: description
    int: errorId

## [post] api/auth/register

Parameters: 
* string: username
* string: password
* string: email

Returns:
* string: token

## [post] api/auth/login

Parameters:
* string: username
* string: password

Returns:
* string: token

---

Everything here now requires a string: token parameter you get from api/login.

## [post] api/post

Parameters:
* string: title
* string: description
* file: file

Returns:
* int: postId

## [get] api/post

Parameters:
* int: postId

Returns:
* string: title
* string: description
* string: iamgeUrl
* ratings[]: ratings
* comments[]: comments

## [post] api/post/comment

Parameters:
* int: postId
* string: text

Returns:
* int: commentId

## [post] api/post/rating

Parameters:
* int: postId
* int: rating (0-5)

Returns:
* int: ratingId

## [post] api/auth/changePassword

Parameters:
* string: oldPassword
* string: newPassword

Returns:
* bool: success

## [post] api/auth/logout

Returns:
* bool: success

## [get] api/browse
* int: offset
* int: 0 < amount < 50
* string: orderBy (date)
* string: order (asc, desc)