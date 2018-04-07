# 🗃 HTML-Parser

A HTML Parser prototype used in [`Layout-Engine`](https://github.com/nersent/layout-engine). The parser can parse incorrect HTML syntax but sometimes it could crash.

## Testing

To test it out, create a file named `index.html` in `HTML-Parser/bin/Debug/Assets` folder.

## Features

* Parsing document from a website or a source code
* Attributes parser
* Auto closing tags
* Ignoring not opened tags

## Examples

* Normal HTML with correct syntax

```html
  <!DOCTYPE html>
  <html>
    <head>
      <meta charset="UTF-8">
      <title>A HTML Parser</title>
    </head>
    <body>
      <div class="container">
        <span>Text</span>
        <br />
        <input type="text">
      </div>
    </body>
  </html>
```

* Not closed tag. It will be closed automatically.

```html
  <!DOCTYPE html>
  <html>
    <head>
      <title>A HTML Parser</title>
    </head>
    <body>
      <div class="container">
        Text
    </body>
  </html>
```

* Ignoring not opened tag.

```html
  <!DOCTYPE html>
  <html>
    <head>
      <title>A HTML Parser</title>
    </head>
    <body>
      <div class="container">
        Text
      </div>
      </div>
      </x>
      </y>
      </z>
    </body>
  </html>
```

## How it works

### 1. It parses source code (string) into different tags. `GetTagsList`

It's searching for brackets (< and >). If it will be found, it adds new tag to tags list. But before it, it's getting tag's content, name e.t.c.

Example source code

```
<html><body><div><span>Text</span></div></body></html>
```

After parsing it will look like this

```html
<html><body><div><span>Text</span></div></body></html>
```

### 2. It parses tags list into DOM tree by leveling. `GetDOMTree`

If a tag is opening, then it adds the tag to parents list. If its closing tag then last parent is removed. Except this there is a lot of other stuff like checking if a tag is must be auto closed and e.t.c.

After DOM parsing

```html
<html>
  <body>
    <div>
      <span>
        Text
      </span>
    </div>
  </body>
</html>
```