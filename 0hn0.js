function Utility() {
    var def = {
        isDoubleTapBug: function(evt, target) {
            if ($.browser.android) {
                if ("touchstart" != evt.type && "touchstart" == $(target).data("lastTouch")) return evt.stopImmediatePropagation(), evt.preventDefault(), !0; - 1 != evt.type.indexOf("touch") && $(target).data("lastTouch", !0)
            }
            return !1
        },
        getEventNames: function(name) {
            switch (name) {
                case "start":
                    return $.browser.ios || $.browser.android ? "touchstart" : "touchstart mousedown";
                case "end":
                    return $.browser.ios || $.browser.android ? "touchend" : "touchend mouseup";
                default:
                    return name
            }
        },
        touchEnded: function() {
            touchEndedSinceTap = !0
        },
        isTouch: function() {
            return "ontouchstart" in document.documentElement
        },
        padLeft: function(nr, n, str) {
            return Array(n - String(nr).length + 1).join(str || "0") + nr
        },
        trim: function(s) {
            return s.replace(/^\s*|\s*$/gi, "")
        },
        between: function(min, max, decimals) {
            return decimals ? 1 * (Math.random() * (max - min) + min).toFixed(decimals) : Math.floor(Math.random() * (max - min + 1) + min)
        },
        shuffleSimple: function(sourceArray) {
            return sourceArray.sort(function() {
                return .5 - Math.random()
            }), sourceArray
        },
        shuffle: function(sourceArray) {
            for (var n = 0; n < sourceArray.length - 1; n++) {
                var k = n + Math.floor(Math.random() * (sourceArray.length - n)),
                    temp = sourceArray[k];
                sourceArray[k] = sourceArray[n], sourceArray[n] = temp
            }
            return sourceArray
        },
        index: function(obj, i) {
            var j = 0;
            for (var name in obj) {
                if (j == i) return obj[name];
                j++
            }
        },
        areArraysEqual: function(arr1, arr2) {
            return !(!arr1 || !arr2) && arr1.join("|") === arr2.join("|")
        },
        count: function(obj) {
            var count = 0;
            for (var name in obj) count++;
            return count
        },
        eat: function(e) {
            return e.preventDefault(), e.stopPropagation(), !1
        },
        pick: function(arr) {
            var drawFromArr = arr;
            if (arr.constructor == Object) {
                drawFromArr = [];
                for (var id in arr) drawFromArr.push(id)
            }
            var drawIndex = Utils.between(0, drawFromArr.length - 1);
            return 0 == drawFromArr.length ? null : drawFromArr[drawIndex]
        },
        draw: function(arr, optionalValueToMatch) {
            var drawFromArr = arr;
            if (arr.constructor == Object) {
                drawFromArr = [];
                for (var id in arr) drawFromArr.push(id)
            }
            if (0 == drawFromArr.length) return null;
            var drawIndex = Utils.between(0, drawFromArr.length - 1);
            if (void 0 != optionalValueToMatch) {
                for (var foundMatch = !1, i = 0; i < drawFromArr.length; i++)
                    if (drawFromArr[i] == optionalValueToMatch) {
                        drawIndex = i, foundMatch = !0;
                        break
                    } if (!foundMatch) return null
            }
            var value = drawFromArr[drawIndex];
            return drawFromArr.splice(drawIndex, 1), value
        },
        removeFromArray: function(arr, val) {
            if (0 == arr.length) return null;
            for (var foundMatch = !1, drawIndex = -1, i = 0; i < arr.length; i++)
                if (arr[i] == val) {
                    drawIndex = i, foundMatch = !0;
                    break
                } if (!foundMatch) return null;
            var value = arr[drawIndex];
            return arr.splice(drawIndex, 1), value
        },
        toArray: function(obj) {
            var arr = [];
            for (var id in obj) arr.push(id);
            return arr
        },
        fillArray: function(min, max, repeatEachValue) {
            repeatEachValue || (repeatEachValue = 1);
            for (var arr = new Array, repeat = 0; repeat < repeatEachValue; repeat++)
                for (var i = min; i <= max; i++) arr.push(i);
            return arr
        },
        contains: function(arr, item) {
            for (var i = 0; i < arr.length; i++)
                if (arr[i] == item) return !0;
            return !1
        },
        setCookie: function(name, value, days) {
            if (days) {
                var date = new Date;
                date.setTime(date.getTime() + 24 * days * 60 * 60 * 1e3);
                expires = "; expires=" + date.toGMTString()
            } else var expires = "";
            document.cookie = name + "=" + value + expires + "; path=/"
        },
        getCookie: function(name) {
            for (var nameEQ = name + "=", ca = document.cookie.split(";"), i = 0; i < ca.length; i++) {
                for (var c = ca[i];
                    " " == c.charAt(0);) c = c.substring(1, c.length);
                if (0 == c.indexOf(nameEQ)) return c.substring(nameEQ.length, c.length)
            }
            return null
        },
        clearCookie: function(name) {
            this.setCookie(name, "", -1)
        },
        cssVendor: function($el, prop, value) {
            switch (prop) {
                case "opacity":
                    $.browser.ie ? $el.css("-ms-filter", '"progid:DXImageTransform.Microsoft.Alpha(Opacity=' + Math.round(100 * value) + ')"') : $el.css(prop, value);
                    break;
                default:
                    for (var prefixes = ["", "-webkit-", "-moz-", "-o-", "-ms-"], i = 0; i < prefixes.length; i++) $el.css(prefixes[i] + prop, value)
            }
        },
        createCSS: function(s, id) {
            id = id || "tempcss", $("#" + id).remove();
            var style = '<style id="' + id + '">' + s + "</style>";
            !window.isWebApp && window.MSApp && window.MSApp.execUnsafeLocalFunction ? MSApp.execUnsafeLocalFunction(function() {
                $("head").first().append($(style))
            }) : $("head").first().append($(style))
        },
        setColorScheme: function(c1, c2) {
            var c2 = c2 || Colors.getComplementary(c1),
                css = (Colors.luminateHex(c1, .05), Colors.luminateHex(c2, .05), ".odd  .tile-1 .inner { background-color: " + c1 + "; }.even .tile-1 .inner { background-color: " + c1 + "; }.odd  .tile-2 .inner { background-color: " + c2 + "; }.even .tile-2 .inner { background-color: " + c2 + "; }");
            Utils.createCSS(css)
        }
    };
    for (var o in def) this[o] = def[o]
}

function opposite(value) {
    switch (value) {
        case Directions.Right:
            return Directions.Left;
        case Directions.Left:
            return Directions.Right;
        case Directions.Up:
            return Directions.Down;
        case Directions.Down:
            return Directions.Up
    }
    return null
}

function generateUUID() {
    var d = (new Date).getTime();
    return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(c) {
        var r = (d + 16 * Math.random()) % 16 | 0;
        return d = Math.floor(d / 16), ("x" == c ? r : 3 & r | 8).toString(16)
    })
}

function Grid(size, height, id) {
    function each(handler) {
        for (var i = 0; i < tiles.length; i++) {
            var x = i % width,
                y = Math.floor(i / width),
                tile = tiles[i];
            if (handler.call(tile, x, y, i, tile)) break
        }
        return self
    }

    function getIndex(x, y) {
        return x < 0 || x >= width || y < 0 || y >= height ? -1 : y * width + x
    }

    function render() {}

    function domRenderer(direct) {
        if (!noRender) {
            if (clearTimeout(renderTOH), direct) {
                Game.debug && console.log("rendering..."), rendered = !1;
                for (var html = '<table data-grid="' + id + '" id="grid" cellpadding=0 cellspacing=0>', y = 0; y < height; y++) {
                    html += "<tr>";
                    for (var x = 0; x < width; x++) {
                        var index = getIndex(x, y),
                            tile = tiles[index],
                            label = "",
                            value = "";
                        switch (tile.type) {
                            case TileType.Value:
                                label = 2, value = tile.value;
                                break;
                            case TileType.Wall:
                                label = 1;
                                break;
                            case TileType.Dot:
                                label = 2
                        }
                        html += '<td data-x="' + x + '" data-y="' + y + '" class="' + ((x + y % 2) % 2 ? "even" : "odd") + '"><div id="tile-' + x + "-" + y + '" class="tile tile-' + label + '"><div class="inner">' + value + "</div></div></td>"
                    }
                    html += "</tr>"
                }
                return html += "</table>", $("#" + id).html(html), Game.resize(), rendered = !0, self
            }
            renderTOH = setTimeout(function() {
                domRenderer(!0)
            }, 0)
        }
    }

    function isDone(allowDots) {
        for (var i = 0; i < tiles.length; i++)
            if (tiles[i].type == TileType.Unknown || !allowDots && tiles[i].type == TileType.Dot) return !1;
        return !0
    }

    function fillDotsAround(aroundTile) {
        overwriteNumbers = !0;
        for (var i = 0; i < size; i++) {
            var tile = tiles[i * size + aroundTile.x];
            tile.type == TileType.Unknown && tile.dot(), tile.type == TileType.Value && overwriteNumbers && tile.dot(), (tile = tiles[aroundTile.y * size + i]).type == TileType.Unknown && tile.dot(), tile.type == TileType.Value && overwriteNumbers && tile.dot()
        }
        return render(), self
    }

    function tile(x, y) {
        return x < 0 || x >= width || y < 0 || y >= height ? null : tiles[getIndex(x, y)]
    }

    function solve(silent, hintMode) {
        var tryAgain = !0,
            attempts = 0,
            hintTile = null,
            pool = tiles;
        for (hintMode && (pool = tiles.concat(), Utils.shuffle(pool)); tryAgain && attempts++ < 99;) {
            if (tryAgain = !1, isDone()) return silent && clearTimeout(renderTOH), !0;
            for (i = 0; i < pool.length; i++) pool[i].info = pool[i].collect();
            for (var tile, info, i = 0; i < pool.length; i++) {
                if (tile = pool[i], info = tile.collect(tile.info), tile.isDot() && !info.unknownsAround && !hintMode) {
                    tile.number(info.numberCount, !0), tryAgain = HintType.NumberCanBeEntered;
                    break
                }
                if (tile.isNumber() && info.unknownsAround) {
                    if (info.numberReached) {
                        hintMode ? hintTile = tile : tile.close(), tryAgain = HintType.ValueReached;
                        break
                    }
                    if (info.singlePossibleDirection) {
                        hintMode ? hintTile = tile : tile.closeDirection(info.singlePossibleDirection, !0, 1), tryAgain = HintType.OneDirectionLeft;
                        break
                    }
                    for (var dir in Directions) {
                        var curDir = info.direction[dir];
                        if (curDir.wouldBeTooMuch) {
                            hintMode ? hintTile = tile : tile.closeDirection(dir), tryAgain = HintType.WouldExceed;
                            break
                        }
                        if (curDir.unknownCount && curDir.numberWhenDottingFirstUnknown + curDir.maxPossibleCountInOtherDirections <= tile.value) {
                            hintMode ? hintTile = tile : tile.closeDirection(dir, !0, 1), tryAgain = HintType.OneDirectionRequired;
                            break
                        }
                    }
                    if (tryAgain) break
                }
                if (tile.isUnknown() && !info.unknownsAround && !info.completedNumbersAround && 0 == info.numberCount) {
                    hintMode ? hintTile = tile : tile.wall(!0), tryAgain = HintType.MustBeWall;
                    break
                }
            }
            if (tileToSolve && tileToSolve.allowQuickSolve && tile && tileToSolve.tile.x == tile.x && tileToSolve.tile.y == tile.y && tileToSolve.exportValue == tile.getExportValue()) return !0;
            if (hintMode) return hint.mark(hintTile, tryAgain), tryAgain = !1, !1
        }
        return render(), silent && clearTimeout(renderTOH), !1
    }

    function save(slot) {
        slot = slot || 1, saveSlots[slot] = {
            values: []
        };
        for (var i = 0; i < tiles.length; i++) saveSlots[slot].values.push(tiles[i].value);
        return self
    }

    function restore(slot) {
        slot = slot || 1;
        var saveSlot = saveSlots[slot];
        if (!saveSlot) return console.log("Cannot restore save slot ", slot), self;
        for (var i = 0; i < saveSlot.values.length; i++) tiles[i].value = saveSlot.values[i];
        return render(), self
    }

    function breakDownWithQuickSolveOrNot(quickSolve) {
        var tile, attempts = 0,
            walls = 0,
            pool = tiles.concat();
        tileToSolve = null, save("full");
        for (i = 0; i < pool.length; i++) tiles[i].isWall() && walls++;
        Utils.shuffle(pool);
        for (var i = 0; i < pool.length && attempts++ < 6;) {
            if (tileToSove = null, (tile = pool[i++]).isWall()) {
                if (1 == walls) continue;
                walls--
            }
            tileToSolve = {
                tile: tile,
                exportValue: tile.getExportValue(),
                allowQuickSolve: quickSolve
            }, tile.clear(), save("breakdown"), solve(!0) ? (restore("breakdown"), attempts = 0) : (restore("breakdown"), tileToSolve.tile.setExportValue(tileToSolve.exportValue), tile.isWall() && walls++)
        }
        tileToSolve = null, save("empty")
    }

    function isValidByCalculation() {
        for (var i = 0; i < tiles.length; i++) {
            var tile = tiles[i];
            if (tile.isEmpty) return !1;
            if (tile.isNumber()) {
                var info = tile.collect();
                if (tile.value != info.numberCount) return console.log(tile.x, tile.y, tile.value, info.numberCount), !1
            }
        }
        return !0
    }

    function getEmptyTiles() {
        var emptyTiles = [];
        return each(function() {
            this.isEmpty && emptyTiles.push(this)
        }), emptyTiles
    }

    function getQuality() {
        return Math.round(getEmptyTiles().length / (width * height) * 100)
    }
    var self = this,
        id = id || "board";
    width = size, height = height || size, tiles = [], saveSlots = {}, renderTOH = 0, rendered = !1, noRender = !1, emptyTile = new Tile(-99, self, -99), currentPuzzle = null, tileToSolve = null;
    var hint = self.hint = new Hint(this);
    this.activateDomRenderer = function() {
        render = this.render = domRenderer, noRender = !1
    }, this.each = each, this.fillDots = function(overwriteNumbers) {
        for (var i = 0; i < tiles.length; i++) {
            var tile = tiles[i];
            tile.type == TileType.Unknown && tile.dot(), tile.type == TileType.Value && overwriteNumbers && tile.dot()
        }
        return render(), self
    }, this.render = render, this.solve = solve, this.number = function(x, y, n) {
        return tile(x, y).number(n), render(), self
    }, this.wall = function(x, y) {
        return tile(x, y).wall(), render(), self
    }, this.dot = function(x, y) {
        return tile(x, y).dot(), render(), self
    }, this.getIndex = getIndex, this.tile = tile, this.load = function(puzzle) {
        currentPuzzle = puzzle, width = puzzle.size, height = puzzle.size, tiles = [];
        for (var i = 0; i < puzzle.empty.length; i++) {
            var tile = new Tile(puzzle.empty[i], self, i, !0);
            tiles.push(tile)
        }
    }, this.generate = function(size) {
        var len = size * size;
        width = height = size;
        for (var i = 0; i < len; i++) {
            var tile = new Tile(2 * (size - 1), self, i);
            tiles.push(tile)
        }
    }, this.maxify = function(maxAllowed) { //Recoge todas las casillas que superen el valor maxAllowed
        for (var tile, tryAgain = !0, attempts = 0, maxAllowed = maxAllowed || width, maxTiles = [], i = 0; i < tiles.length; i++)(tile = tiles[i]).value > maxAllowed && maxTiles.push(tile);
        Utils.shuffle(maxTiles); //Las baraja?
        for (; tryAgain && attempts++ < 99;)
            for (tryAgain = !1, i = 0; i < maxTiles.length; i++) {
                var x = maxTiles[i].x,
                    y = maxTiles[i].y;
                if ((tile = tiles[y * size + x]).value > maxAllowed) {
                    var max = maxAllowed,
                        cuts = tile.getTilesInRange(1, max),
                        cut = null,
                        firstCut = null;
                    for (Utils.shuffle(cuts); !cut && cuts.length;) cut = cuts.pop(), firstCut || (firstCut = cut);
                    cut || (cut = firstCut), cut ? (cut.wall(!0), fillDotsAround(cut), solve(), tryAgain = !0) : console.log("no cut found for", tile.x, tile.y, tile.value, cuts, 1, max);
                    break
                }
            }
        render()
    }, this.save = save, this.restore = restore, this.breakDown = function() {
        breakDownWithQuickSolveOrNot(!0), solve() || (restore("full"), breakDownWithQuickSolveOrNot(!1)), restore("empty")
    }, this.clear = function() {
        each(function(x, y, i, tile) {
            tile.clear()
        })
    }, this.getNextLockedInTile = function() {
        var lockedInTiles = [];
        return each(function(x, y, i, tile) {
            tile.isLockedIn() && lockedInTiles.push(tile)
        }), !!lockedInTiles.length && Utils.pick(lockedInTiles)
    }, this.getValues = function() {
        var values = [];
        return each(function() {
            values.push(this.getExportValue())
        }), values
    }, this.isValid = function(hard) {
        if (hard) return isValidByCalculation();
        for (var i = 0; i < tiles.length; i++) {
            var tile = tiles[i];
            if (!tile.isEmpty)
                if (1 == currentPuzzle.full[i]) {
                    if (!tile.isWall()) return !1
                } else if (currentPuzzle.full[i] > 1 && !tile.isNumberOrDot()) return !1
        }
        return !0
    }, this.mark = function(x, y) {
        return tile(x, y).mark(), self
    }, this.unmark = function(x, y) {
        if ("number" == typeof x && "number" == typeof y) return tile(x, y).unmark(), self;
        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++) tile(x, y).unmark();
        return self
    }, this.getClosedWrongTiles = function() {
        for (var pool = tiles, wrongTiles = [], i = 0; i < pool.length; i++) pool[i].info = pool[i].collect();
        for (i = 0; i < pool.length; i++) {
            var tile = pool[i],
                info = tile.collect(tile.info);
            tile.isNumber() && info.numberCount != tile.value && 0 == info.unknownsAround && wrongTiles.push(tile)
        }
        return wrongTiles
    }, this.__defineGetter__("tiles", function() {
        return tiles
    }), this.__defineGetter__("width", function() {
        return width
    }), this.__defineGetter__("height", function() {
        return height
    }), this.__defineGetter__("rendered", function() {
        return rendered
    }), this.__defineGetter__("quality", function() {
        return getQuality()
    }), this.__defineGetter__("emptyTileCount", function() {
        return getEmptyTiles().length
    }), this.__defineGetter__("emptyTiles", function() {
        return getEmptyTiles()
    }), this.__defineGetter__("hint", function() {
        return hint
    })
}

function Tile(tileValue, grid, index, isExportValue) {
    function setValue(tileValue) {
        return -2 == tileValue ? (value = tileValue, type = TileType.Dot) : isNaN(tileValue) || tileValue < 0 || tileValue > 90 ? (value = -1, type = TileType.Unknown) : (value = tileValue, type = 0 == tileValue ? TileType.Wall : TileType.Value), render(), value
    }

    function render() {
        if (grid.render) {
            if (grid.rendered) {
                var $tile = $("#tile-" + x + "-" + y),
                    label = "",
                    value = "";
                switch (type) {
                    case TileType.Value:
                        label = 2, value = tile.value;
                        break;
                    case TileType.Wall:
                        label = 1;
                        break;
                    case TileType.Dot:
                        label = 2
                }
                $tile.removeClass().addClass("tile tile-" + label), $tile.find(".inner").text(value)
            } else grid.render();
            return self
        }
    }

    function setType(tileType) {
        switch (tileType) {
            case TileType.Unknown:
                type = tileType, value = -1;
                break;
            case TileType.Wall:
                type = tileType, value = 0;
                break;
            case TileType.Dot:
                type = tileType, value = -2;
                break;
            case TileType.Value:
                console.log("Error. Don't set tile type directly to TileType.Value.")
        }
        render()
    }

    function isDot() {
        return type == TileType.Dot
    }

    function isNumber() {
        return type == TileType.Value
    }

    function dot() {
        setType(TileType.Dot);
        return self
    }

    function wall() {
        setType(TileType.Wall);
        return self
    }

    function unknown() {
        setType(TileType.Unknown);
        return self
    }

    function traverse(hor, ver) {
        var newX = x + hor,
            newY = y + ver;
        return grid.tile(newX, newY)
    }

    function closeDirection(dir, withDots, amount) {
        for (var t = self.move(dir), count = 0; t && !t.isWall(); t = t.move(dir)) {
            if (t.isUnknown()) {
                if (count++, !withDots) {
                    t.wall(!0);
                    break
                }
                t.dot(!0)
            }
            if (count >= amount) break
        }
    }

    function setExportValue(value) {
        switch (value) {
            case 0:
                unknown();
                break;
            case 1:
                wall();
                break;
            case 2:
                dot();
                break;
            default:
                setValue(value - 2)
        }
    }
    var self = this,
        value = -1,
        type = TileType.Unknown,
        x = this.x = index % grid.width,
        y = this.y = Math.floor(index / grid.width);
    this.id = x + "," + y;
    isExportValue ? setExportValue(tileValue) : setValue(tileValue), this.mark = function() {
        return $("#tile-" + x + "-" + y).addClass("marked"), self
    }, this.unmark = function() {
        return $("#tile-" + x + "-" + y).removeClass("marked"), self
    }, this.dot = dot, this.wall = wall, this.number = function(n) {
        setValue(n);
        return self
    }, this.unknown = unknown, this.isDot = isDot, this.isWall = function() {
        return type == TileType.Wall
    }, this.isNumber = isNumber, this.isNumberOrDot = function() {
        return isNumber() || isDot()
    }, this.isUnknown = function() {
        return type == TileType.Unknown
    }, this.isLockedIn = function() {
        if (!isDot()) return !1;
        for (var dir in Directions)
            if (self.move(dir) && !self.move(dir).isWall()) return !1;
        return !0
    }, this.collect = function(info) {
        if (info)
            for (var dir in Directions) {
                for (var curDir = info.direction[dir], t = self.move(dir); t && !t.isWall(); t = t.move(dir)) t.isNumber() && t.info.numberReached && (info.completedNumbersAround = !0);
                if (isNumber() && !info.numberReached && curDir.unknownCount) {
                    curDir.maxPossibleCountInOtherDirections = 0;
                    for (var otherDir in Directions) otherDir != dir && (curDir.maxPossibleCountInOtherDirections += info.direction[otherDir].maxPossibleCount)
                }
            } else {
                info = {
                    unknownsAround: 0,
                    numberCount: 0,
                    numberReached: !1,
                    canBeCompletedWithUnknowns: !1,
                    completedNumbersAround: !1,
                    singlePossibleDirection: null,
                    direction: {}
                };
                for (var dir in Directions) info.direction[dir] = {
                    unknownCount: 0,
                    numberCountAfterUnknown: 0,
                    wouldBeTooMuch: !1,
                    maxPossibleCount: 0,
                    maxPossibleCountInOtherDirections: 0,
                    numberWhenDottingFirstUnknown: 0
                };
                var lastPossibleDirection = null,
                    possibleDirCount = 0;
                for (var dir in Directions)
                    for (t = self.move(dir); t && !t.isWall(); t = t.move(dir)) curDir = info.direction[dir], t.isUnknown() ? (curDir.unknownCount || curDir.numberWhenDottingFirstUnknown++, curDir.unknownCount++, curDir.maxPossibleCount++, info.unknownsAround++, isNumber() && lastPossibleDirection != dir && (possibleDirCount++, lastPossibleDirection = dir)) : (t.isNumber() || t.isDot()) && (curDir.maxPossibleCount++, curDir.unknownCount ? isNumber() && 1 == curDir.unknownCount && (curDir.numberCountAfterUnknown++, curDir.numberWhenDottingFirstUnknown++, curDir.numberCountAfterUnknown + 1 > value && (curDir.wouldBeTooMuch = !0)) : (info.numberCount++, curDir.numberWhenDottingFirstUnknown++));
                1 == possibleDirCount && (info.singlePossibleDirection = lastPossibleDirection), isNumber() && value == info.numberCount ? info.numberReached = !0 : isNumber() && value == info.numberCount + info.unknownsAround && (info.canBeCompletedWithUnknowns = !0)
            }
        return 1 == possibleDirCount && (info.singlePossibleDirection = lastPossibleDirection), isNumber() && value == info.numberCount ? info.numberReached = !0 : isNumber() && value == info.numberCount + info.unknownsAround && (info.canBeCompletedWithUnknowns = !0), info
    }, this.traverse = traverse, this.right = function() {
        return traverse(1, 0)
    }, this.left = function() {
        return traverse(-1, 0)
    }, this.up = function() {
        return traverse(0, -1)
    }, this.down = function() {
        return traverse(0, 1)
    }, this.move = function(dir) {
        switch (dir) {
            case Directions.Right:
                return traverse(1, 0);
            case Directions.Left:
                return traverse(-1, 0);
            case Directions.Up:
                return traverse(0, -1);
            case Directions.Down:
                return traverse(0, 1)
        }
    }, this.close = function(withDots) {
        for (var dir in Directions) closeDirection(dir, withDots)
    }, this.clear = function() {
        unknown()
    }, this.render = render, this.closeDirection = closeDirection, this.getTilesInRange = function(min, max) {
        var self = this,
            result = [],
            max = max || min;
        for (var dir in Directions)
            for (var distance = 0, t = self.move(dir); t && !t.isWall(); t = t.move(dir)) ++distance >= min && distance <= max && result.push(t);
        return result
    }, this.getExportValue = function() {
        switch (type) {
            case TileType.Unknown:
                return 0;
            case TileType.Wall:
                return 1;
            case TileType.Dot:
                return 2;
            case TileType.Value:
                return value + 2
        }
    }, this.setExportValue = setExportValue, this.__defineGetter__("value", function() {
        return value
    }), this.__defineSetter__("value", function(v) {
        return setValue(v)
    }), this.__defineGetter__("type", function() {
        return type
    }), this.__defineSetter__("type", function(v) {
        return setValue(v)
    }), this.__defineGetter__("isEmpty", function() {
        return -1 == value
    })
}

function Hint(grid) {
    function show(type, arg1) {
        var s = type,
            translated = Language.get(s);
        translated && (s = translated), arg1 && (s = s.replace(/\%s/gi, arg1)), $("#hintMsg").html("<span>" + s + "</span>"), $("html").addClass("showHint"), visible = !0, type == HintType.LockedIn && setTimeout(function() {
            PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.locked_in)
        }, 2500)
    }

    function hide() {
        $("html").removeClass("showHint"), visible = !1
    }
    var active = !1,
        visible = !1,
        info = {
            type: HintType.None,
            tile: null
        };
    this.clear = function() {
        hide(), grid && grid.unmark(), active = !1, info = {
            type: HintType.None,
            tile: null
        }
    }, this.mark = function(tile, hintType) {
        return !!active && (info.tile = tile, info.type = hintType, !0)
    }, this.next = function() {
        var wrongTiles = grid.getClosedWrongTiles();
        if (wrongTiles.length) {
            var wrongTile = Utils.pick(wrongTiles),
                hintType = wrongTile.collect().numberCount > wrongTile.value ? HintType.ErrorClosedTooLate : HintType.ErrorClosedTooEarly;
            return wrongTile.mark(), void show(hintType)
        }
        var lockedInTile = grid.getNextLockedInTile();
        if (lockedInTile) return show(HintType.LockedIn), void lockedInTile.mark();
        active = !0, grid.solve(!1, !0), info.tile && (show(info.type), info.tile.mark())
    }, this.show = show, this.hide = hide, this.info = info, this.__defineGetter__("active", function() {
        return active
    }), this.__defineSetter__("active", function(v) {
        active = v
    }), this.__defineGetter__("visible", function() {
        return visible
    })
}

function generateGridAndSolution(size) {
    for (var puzzle, grid = null, attempts = 0; attempts++ < 10;) {
        grid = new Grid(size), puzzle = {
            size: size,
            full: [],
            empty: [],
            quality: 0,
            ms: 0
        };
        var d = new Date;
        if (grid.clear(), grid.generate(size), grid.maxify(size), grid.isValid(!0)) {
            puzzle.full = grid.getValues(), grid.breakDown(), puzzle.empty = grid.getValues(), puzzle.ms = new Date - d, puzzle.quality = grid.quality;
            break
        }
        grid = null
    }
    self.postMessage(JSON.stringify(puzzle))
}
var Config = {
    unlimited: !0,
    daily: !0,
    colorTheme: !0,
    languageSwitch: !0,
    playLabelPlay: !1,
    playLabelFree: !0,
    newLabel: !0,
    GoogleAnalytics: "UA-45298460-7",
    defaultLanguage: "en",
    lang: {
        nl: {
            tweetMessage: "Ik heb zojuist een $size x $size #0hn0 puzzel opgelost en mijn score is nu $score. http://0hn0.com ",
            tweetMessageDaily: "Ik heb zojuist de $size x $size #0hn0 puzzel van $today opgelost en mijn score is nu $score. http://0hn0.com "
        },
        en: {
            tweetMessage: "I just completed a $size x $size #0hn0 puzzle and my score is $score. http://0hn0.com ",
            tweetMessageDaily: "I just completed $today's $size x $size #0hn0 puzzle and my score is $score. http://0hn0.com "
        }
    }
};
window.isWebApp = !0;
var app = {
    fontsLoaded: !1,
    deviceReady: !1,
    started: !1,
    initialize: function() {
        this.bindEvents()
    },
    bindEvents: function() {
        document.addEventListener("deviceready", this.onDeviceReady, !1)
    },
    onDeviceReady: function() {
        setTimeout(function() {
            navigator && navigator.splashscreen && navigator.splashscreen.hide()
        }, 100), app.deviceReady = !0, app.startTheGameIfWeCan(), PlayCenter.autoSignIn()
    },
    receivedEvent: function(id) {},
    fontsLoaded: function() {
        app.fontsLoaded = !0
    },
    startTheGameIfWeCan: function() {
        if (app.started) return !1;
        app.started = !0, Game.init(), Game.start()
    }
};
app.initialize(),
    function(a, b) {
        function c(c, j, k) {
            var n = [],
                s = g(f((j = 1 == j ? {
                    entropy: !0
                } : j || {}).entropy ? [c, i(a)] : null == c ? h() : c, 3), n),
                t = new d(n),
                u = function() {
                    for (var a = t.g(m), b = p, c = 0; a < q;) a = (a + c) * l, b *= l, c = t.g(1);
                    for (; a >= r;) a /= 2, b /= 2, c >>>= 1;
                    return (a + c) / b
                };
            return u.int32 = function() {
                return 0 | t.g(4)
            }, u.quick = function() {
                return t.g(4) / 4294967296
            }, u.double = u, g(i(t.S), a), (j.pass || k || function(a, c, d, f) {
                return f && (f.S && e(f, t), a.state = function() {
                    return e(t, {})
                }), d ? (b[o] = a, c) : a
            })(u, s, "global" in j ? j.global : this == b, j.state)
        }

        function d(a) {
            var b, c = a.length,
                d = this,
                e = 0,
                f = d.i = d.j = 0,
                g = d.S = [];
            for (c || (a = [c++]); e < l;) g[e] = e++;
            for (e = 0; e < l; e++) g[e] = g[f = s & f + a[e % c] + (b = g[e])], g[f] = b;
            (d.g = function(a) {
                for (var b, c = 0, e = d.i, f = d.j, g = d.S; a--;) b = g[e = s & e + 1], c = c * l + g[s & (g[e] = g[f = s & f + b]) + (g[f] = b)];
                return d.i = e, d.j = f, c
            })(l)
        }

        function e(a, b) {
            return b.i = a.i, b.j = a.j, b.S = a.S.slice(), b
        }

        function f(a, b) {
            var c, d = [],
                e = typeof a;
            if (b && "object" == e)
                for (c in a) try {
                    d.push(f(a[c], b - 1))
                } catch (a) {}
            return d.length ? d : "string" == e ? a : a + "\0"
        }

        function g(a, b) {
            for (var c, d = a + "", e = 0; e < d.length;) b[s & e] = s & (c ^= 19 * b[s & e]) + d.charCodeAt(e++);
            return i(b)
        }

        function h() {
            try {
                var b;
                return j && (b = j.randomBytes) ? b = b(l) : (b = new Uint8Array(l), (k.crypto || k.msCrypto).getRandomValues(b)), i(b)
            } catch (b) {
                var c = k.navigator,
                    d = c && c.plugins;
                return [+new Date, k, d, k.screen, i(a)]
            }
        }

        function i(a) {
            return String.fromCharCode.apply(0, a)
        }
        var j, k = this,
            l = 256,
            m = 6,
            o = "random",
            p = b.pow(l, m),
            q = b.pow(2, 52),
            r = 2 * q,
            s = l - 1;
        if (b["seed" + o] = c, g(b.random(), a), "object" == typeof module && module.exports) {
            module.exports = c;
            try {
                j = require("crypto")
            } catch (a) {}
        } else "function" == typeof define && define.amd && define(function() {
            return c
        })
    }([], Math);
var Utils = new Utility,
    Colors = new function() {
        function hexToRgb(hex) {
            var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
            return result ? {
                r: parseInt(result[1], 16),
                g: parseInt(result[2], 16),
                b: parseInt(result[3], 16)
            } : null
        }

        function componentToHex(c) {
            var hex = c.toString(16);
            return 1 == hex.length ? "0" + hex : hex
        }

        function rgbToHex(r, g, b) {
            return "object" == typeof r && (g = r.g, b = r.b, r = r.r), "#" + componentToHex(r) + componentToHex(g) + componentToHex(b)
        }

        function rgbToHsv(rgb) {
            return hsv = new Object, max = max3(rgb.r, rgb.g, rgb.b), dif = max - min3(rgb.r, rgb.g, rgb.b), hsv.saturation = 0 == max ? 0 : 100 * dif / max, 0 == hsv.saturation ? hsv.hue = 0 : rgb.r == max ? hsv.hue = 60 * (rgb.g - rgb.b) / dif : rgb.g == max ? hsv.hue = 120 + 60 * (rgb.b - rgb.r) / dif : rgb.b == max && (hsv.hue = 240 + 60 * (rgb.r - rgb.g) / dif), hsv.hue < 0 && (hsv.hue += 360), hsv.value = Math.round(100 * max / 255), hsv.hue = Math.round(hsv.hue), hsv.saturation = Math.round(hsv.saturation), hsv
        }

        function hsvToRgb(hsv) {
            var rgb = new Object;
            if (0 == hsv.saturation) rgb.r = rgb.g = rgb.b = Math.round(2.55 * hsv.value);
            else {
                switch (hsv.hue /= 60, hsv.saturation /= 100, hsv.value /= 100, i = Math.floor(hsv.hue), f = hsv.hue - i, p = hsv.value * (1 - hsv.saturation), q = hsv.value * (1 - hsv.saturation * f), t = hsv.value * (1 - hsv.saturation * (1 - f)), i) {
                    case 0:
                        rgb.r = hsv.value, rgb.g = t, rgb.b = p;
                        break;
                    case 1:
                        rgb.r = q, rgb.g = hsv.value, rgb.b = p;
                        break;
                    case 2:
                        rgb.r = p, rgb.g = hsv.value, rgb.b = t;
                        break;
                    case 3:
                        rgb.r = p, rgb.g = q, rgb.b = hsv.value;
                        break;
                    case 4:
                        rgb.r = t, rgb.g = p, rgb.b = hsv.value;
                        break;
                    default:
                        rgb.r = hsv.value, rgb.g = p, rgb.b = q
                }
                rgb.r = Math.round(255 * rgb.r), rgb.g = Math.round(255 * rgb.g), rgb.b = Math.round(255 * rgb.b)
            }
            return rgb
        }

        function hueShift(h, s) {
            for (h += s; h >= 360;) h -= 360;
            for (; h < 0;) h += 360;
            return h
        }

        function min3(a, b, c) {
            return a < b ? a < c ? a : c : b < c ? b : c
        }

        function max3(a, b, c) {
            return a > b ? a > c ? a : c : b > c ? b : c
        }
        this.hexToRgb = hexToRgb, this.componentToHex = componentToHex, this.rgbToHex = rgbToHex, this.colorToRgb = function(color) {
            return isNaN(color) || (color = PALETTE[color]), hexToRgb(color)
        }, this.colorsMatch = function(c1, c2) {
            return c1.r == c2.r && c1.g == c2.g && c1.b == c2.b
        }, this.getComplementary = function(rgb) {
            var asHex = !1;
            "string" == typeof rgb && (asHex = !0), asHex && (rgb = hexToRgb(rgb));
            var comp = rgbToHsv(rgb);
            comp.hue = hueShift(comp.hue, 180);
            var result = hsvToRgb(comp);
            return asHex && (result = rgbToHex(result)), result
        }, this.rgbToHsv = rgbToHsv, this.hsvToRgb = hsvToRgb, this.luminateHex = function(hex, lum) {
            (hex = String(hex).replace(/[^0-9a-f]/gi, "")).length < 6 && (hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2]), lum = lum || 0;
            var c, i, rgb = "#";
            for (i = 0; i < 3; i++) c = parseInt(hex.substr(2 * i, 2), 16), rgb += ("00" + (c = Math.round(Math.min(Math.max(0, c + c * lum), 255)).toString(16))).substr(c.length);
            return rgb
        }
    };
window.$ = window.$ || {}, $.browser = {}, $.browser.chrome = /chrome/.test(navigator.userAgent.toLowerCase()), $.browser.android = /android/.test(navigator.userAgent.toLowerCase()), $.browser.safari = /safari/.test(navigator.userAgent.toLowerCase()), $.browser.ipad = /ipad/.test(navigator.userAgent.toLowerCase()), $.browser.iphone = /iphone|ipod/.test(navigator.userAgent.toLowerCase()), $.browser.ios = /ipad|iphone|ipod/.test(navigator.userAgent.toLowerCase()), $.browser.ie = /msie/.test(navigator.userAgent.toLowerCase()), $.browser.chromeWebStore = !(!window.chrome || !window.chrome.storage);
for (var o in $.browser) $.browser[o] && $("html").addClass(o);
window.requestAnimFrame = window.requestAnimationFrame || window.webkitRequestAnimationFrame || window.mozRequestAnimationFrame || window.oRequestAnimationFrame || window.msRequestAnimationFrame || function(callback, element) {
    window.setTimeout(function() {
        callback(+new Date)
    }, 10)
}, window.cancelAnimFrame = window.cancelAnimationFrame || window.webkitCancelRequestAnimationFrame || window.mozCancelRequestAnimationFrame || window.oCancelRequestAnimationFrame || window.msCancelRequestAnimationFrame || function() {};
var HintType = {
        None: "None",
        NumberCanBeEntered: "NumberCanBeEntered",
        OneDirectionLeft: "OneDirectionLeft",
        ValueReached: "ValueReached",
        WouldExceed: "WouldExceed",
        OneDirectionRequired: "OneDirectionRequired",
        MustBeWall: "MustBeWall",
        ErrorClosedTooEarly: "ErrorClosedTooEarly",
        ErrorClosedTooLate: "ErrorClosedTooLate",
        Error: "Error",
        Errors: "Errors",
        LockedIn: "LockedIn",
        GameContinued: "GameContinued",
        TimeTrialShown: "TimeTrialShown"
    },
    TileType = {
        Unknown: "Unknown",
        Dot: "Dot",
        Wall: "Wall",
        Value: "Value"
    },
    Directions = {
        Left: "Left",
        Right: "Right",
        Up: "Up",
        Down: "Down"
    },
    Game = new function() {
        function addNativeSocialHooks() {
            window.plugins && window.plugins.socialsharing || SocialSharing.install(), tweet = !0, facebook = !0, $("#tweeturl").on(Utils.getEventNames("click"), function(evt) {
                return evt.stopPropagation(), evt.preventDefault(), setTimeout(function() {
                    window.plugins.socialsharing.shareViaTwitter(shareMsg), PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.how_very_social_of_you)
                }, 0), !1
            }), $("#facebook").on(Utils.getEventNames("click"), function(evt) {
                return evt.stopPropagation(), evt.preventDefault(), setTimeout(function() {
                    window.plugins.socialsharing.shareViaFacebook(shareMsg), PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.how_very_social_of_you)
                }, 0), !1
            })
        }

        function touchSplashScreen() {
            if (!skipSplash && !Storage.getDataValue("splashSkibbable", !1)) {
                if (++splashScreenTouched < 8) return;
                Storage.setDataValue("splashSkibbable", !0), PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.the_impatient_one)
            }
            checkTutorialPlayed(showMenu)
        }

        function resize() {
            var devices = {
                iphone4: {
                    width: 320,
                    height: 480
                },
                iphone5: {
                    width: 320,
                    height: 568
                },
                nexus5: {
                    width: 360,
                    height: 640
                },
                nexus7: {
                    width: 604,
                    height: 966
                }
            };
            for (var id in devices) devices[id].id = id;
            var viewport = {
                    width: $("#feelsize").width(),
                    height: $("#feelsize").height()
                },
                aspectRatio = viewport.width / viewport.height,
                desiredType = "iphone4",
                desired = devices[desiredType],
                closest = 999;
            for (var type in devices) {
                var curDevice = devices[type],
                    deviceAspectRatio = curDevice.width / curDevice.height,
                    difference = Math.abs(aspectRatio - deviceAspectRatio);
                difference < closest && (desiredType = type, desired = devices[type], closest = difference)
            }
            var sizeToWidth = viewport.width / viewport.height < aspectRatio,
                box = {
                    width: Math.floor(sizeToWidth ? viewport.width : viewport.height / desired.height * desired.width),
                    height: Math.floor(sizeToWidth ? viewport.width / desired.width * desired.height : viewport.height)
                };
            $("#container").css({
                width: box.width + "px",
                height: box.height + "px"
            });
            var containerSize = box.width;
            $("h1").css("font-size", Math.round(.24 * containerSize) + "px"), $("h2").css("font-size", Math.round(.18 * containerSize) + "px"), $("h3").css("font-size", Math.round(.15 * containerSize) + "px"), $("p").css("font-size", Math.round(.07 * containerSize) + "px"), $(".by").css("font-size", Math.round(.05 * containerSize) + "px"), $("#menu h2").css("font-size", Math.round(.24 * containerSize) + "px"), $("#menu p").css("font-size", Math.round(.1 * containerSize) + "px"), $("#menu p").css("padding", Math.round(.05 * containerSize) + "px 0"), $("#menu p").css("line-height", Math.round(.08 * containerSize) + "px"), $("#menu p .multiline").css("line-height", Math.round(.12 * containerSize) + "px"), $("#hiddendigit, #timer .digit, #percentage").css("font-size", Math.round(.05 * containerSize) + "px");
            var scoreSize = Math.round(.1 * containerSize);
            $("#score").css({
                "font-size": scoreSize + "px",
                "line-height": .85 * scoreSize + "px",
                height: scoreSize + "px"
            });
            var iconSize = Math.floor(.075 * containerSize);
            $(".icon").css({
                width: iconSize,
                height: iconSize,
                marginLeft: .7 * iconSize,
                marginRight: .7 * iconSize
            }), $(".board table").each(function(i, el) {
                var $el = $(el),
                    w = ($el.attr("data-grid"), $el.width()),
                    size = $el.find("tr").first().children("td").length,
                    tileSize = Math.floor(w / size);
                tileSize && $el.find(".tile").css({
                    width: tileSize,
                    height: tileSize,
                    "line-height": Math.round(.9 * tileSize) + "px",
                    "font-size": Math.round(.5 * tileSize) + "px"
                })
            }), $("#digits").width($("#titlegrid table").width()).height($("#titlegrid table").height()), $("#digits").css({
                "line-height": Math.round(.92 * $("#titlegrid table").height()) + "px",
                "font-size": .5 * $("#titlegrid table").height() + "px"
            });
            var topVSpace = Math.floor($("#container").height() / 2 - $("#board").height() / 2);
            $("#hintMsg").height(topVSpace + "px"), $(".digit").css("width", $("#hiddendigit").width() + "px"), window.Themes && Themes.resize(box.width, box.height)
        }

        function showTitleScreen() {
            onHomeScreen = !0, $(".screen").hide().removeClass("show"), $("#title").show(), setTimeout(function() {
                $("#title").addClass("show")
            }, 0)
        }

        function showGame() {
            onHomeScreen = !1, $(".screen").hide().removeClass("show"), $("#game").show(), $("#game #time").show(), setTimeout(function() {
                $("#game").addClass("show")
            }, 0), resize(), currentPuzzle && !currentPuzzle.isTutorial && ($("#bar [data-action]").show(), $('#bar [data-action="playcenter"]').hide(), $('#bar [data-action="continue"]').hide(), $('#bar [data-action="achievements"]').hide(), $('#bar [data-action="leaderboards"]').hide(), $('#tweeturl, #facebook, [data-action="apps"]').hide())
        }

        function showMenu() {
            inGame = !1, inText = !1, onHomeScreen = !0, clearTimeouts(), clearTimeout(removeSpinTOH), $("#playcenter").removeClass("spin"), $(".screen").hide().removeClass("show"), $("#menu").show(), $("#bar").show(), $("#bar [data-action]").hide(), $('#bar [data-action="show-lovie"]').show(), PlayCenter.enabled && $('#bar [data-action="playcenter"]').show(), getScore(function(value) {
                $("#scorenr").html(value)
            }), setTimeout(function() {
                $("#menu").addClass("show")
            }, 0), resize()
        }

        function showAbout() {
            onHomeScreen = !1, inText = !0, $(".screen").hide().removeClass("show"), $("#about").show(), $("#bar [data-action]").hide(), setTimeout(function() {
                $("#about").addClass("show")
            }, 0), resize(), window.Marker && Marker.save("page", "about")
        }

        function showOnline() {
            onHomeScreen = !1, inText = !0, $(".screen").hide().removeClass("show"), $("#online").show(), $("#bar [data-action]").hide(), $('#bar [data-action="back"]').show(), setTimeout(function() {
                $("#online").addClass("show")
            }, 0), resize(), window.Marker && Marker.save("page", "online")
        }

        function showRules() {
            onHomeScreen = !1, inText = !0, $(".screen").hide().removeClass("show"), $("#rules").show(), setTimeout(function() {
                $("#rules").addClass("show")
            }, 0), resize(), window.Marker && Marker.save("page", "rules")
        }

        function showThanks() {
            onHomeScreen = !1, inText = !0, $(".screen").hide().removeClass("show"), $("#thanks").show(), setTimeout(function() {
                $("#thanks").addClass("show")
            }, 0), resize(), window.Marker && Marker.save("page", "thanks")
        }

        function showApps() {
            onHomeScreen = !1, inText = !0, $(".screen").hide().removeClass("show"), $("#apps").show(), setTimeout(function() {
                $("#apps").addClass("show")
            }, 0), resize(), window.Marker && Marker.save("page", "apps")
        }

        function showSettings() {
            onHomeScreen = !1, inText = !0, $(".screen").hide().removeClass("show"), $("#settings").show(), $("#settings [data-action]").show(), setTimeout(function() {
                $("#settings").addClass("show")
            }, 0), resize(), window.Marker && Marker.save("page", "settings")
        }

        function showSizes() {
            onHomeScreen = !1, inText = !1, $("#donate").hide(), showGame(), dailyMode ? ($("#boardsize").html("<span>" + today() + "</span>"), $("#chooseSize").addClass("show")) : (window.Config || {}).playLabelFree ? ($("#boardsize").html("<span>" + Language.get("Free play") + "</span>"), $("#chooseSize").addClass("show")) : $("#boardsize").html("<span>" + Language.get("Select a size") + "</span>"), updateDailyModeCheckmarks(), $(".menugrid").removeClass("hidden"), $("#board").addClass("hidden"), $("#bar [data-action]").hide(), $('#bar [data-action="back"]').show(), PlayCenter.isSignedIn && $('[data-action="achievements"],[data-action="leaderboards"]').show(), continueLastGame && !currentPuzzle.isTutorial && $('[data-action="continue"]').show().addClass("subtleHintOnce"), $("#board").addClass("hidden"), $("#score").show(), $("#time").hide(), setTimeout(function() {
                grid && grid.clear(), $("#score").addClass("show")
            }, 0)
        }

        function showLoad() {
            onHomeScreen = !1, $(".screen").hide().removeClass("show"), $("#loading").show(), setTimeout(function() {
                $("#loading").addClass("show")
            }, 0)
        }

        function loadGame(size) {
            onHomeScreen = !1, $("#game").removeClass("show"), showLoad(), resize(), setTimeout(function() {
                if (dailyMode) {
                    var seed = getTodayStr() + "-" + puzzle;
                    Math.seedrandom(seed)
                } else Math.seedrandom();
                var forceGenerated = !!dailyMode,
                    puzzle = Levels.getSize(size, forceGenerated);
                startGame(puzzle)
            }, 100)
        }

        function startGame(puzzle, isContinued) {
            if (isNaN(puzzle)) {
                if (onHomeScreen = !1, !puzzle || !puzzle.size || !puzzle.full) throw "no proper puzzle object received";
                debug && console.log(puzzle), clearTimeouts(), window.STOPPED || (startedTutorial = !1, $("#undo").closest(".iconcon").css("display", "inline-block"), $(".menugrid").addClass("hidden"), $("#board").removeClass("hidden"), $("#chooseSize").removeClass("show"), $("#score").removeClass("show").hide(), $('#bar [data-action="help"]').removeClass("hidden wiggle"), $('#bar [data-action="help"]').removeClass("subtleHint").css("display", "inline-block"), $("#boardsize").html("<span>" + puzzle.size + " x " + puzzle.size + "</span>"), grid = new Grid(puzzle.size, puzzle.size), lastSize = puzzle.size, continueLastGame = !0, inGame = !0, inText = !1, timeInSeconds = 0, hintsUsed = 0, undosUsed = 0, locksToggled = 0, systemTilesLockToggleable = !0, grid.load(puzzle), grid.each(function() {
                    this.value = this.value, (this.isWall() || this.isNumber()) && (this.system = !0)
                }), currentPuzzle = puzzle, grid.hint.active = !0, grid.activateDomRenderer(), grid.render(), undoStack = [], undone = !1, gameEnded = !1, puzzle.isTutorial || window.Marker && Marker.save("level", "start", puzzle.size), isContinued || (time = new Date), puzzle.isTutorial ? $("#game").addClass("isTutorial") : $("#game").removeClass("isTutorial"), tapBoardSizeToSolve && time.setSeconds(time.getSeconds() - puzzle.size * puzzle.size * 5), clearTimeout(timerTOH), showBestTime(puzzle.size), updateTime(), updatePercentage(!0), showTime && !puzzle.isTutorial ? toggleTimeTrial(!0, !0) : toggleTimeTrial(!1, !0), setTimeout(function() {
                    showGame()
                }, 0))
            } else loadGame(puzzle)
        }

        function toggleTimeTrial(forceState, isTemporary) {
            if (showTime = void 0 != forceState ? forceState : !showTime, isTemporary || Storage.setDataValue("showTimeTrial", showTime), showTime) {
                if ($("#time").removeClass("hidden"), isTemporary || $("#toggleTimeTrialValue text").text(Language.get("Yes")), inGame) {
                    $("#time").show();
                    var timeStr = showBestTime(currentPuzzle.size);
                    void 0 == forceState && grid.hint.show(HintType.TimeTrialShown, timeStr)
                }
            } else $("#time").addClass("hidden"), isTemporary || $("#toggleTimeTrialValue text").text(Language.get("No")), inGame && ($("#time").hide(), void 0 == forceState && grid.hint.hide(), $("#boardsize").html("<span>" + currentPuzzle.size + " x " + currentPuzzle.size + "</span>"))
        }

        function showBestTime(size) {
            var bestSeconds = Storage.getDataValue("bestTimeSize" + size, 60 * size);
            if (!bestSeconds || 0 === bestSeconds || bestSeconds > 60 * size) return !1;
            var ms = 1e3 * bestSeconds,
                seconds = parseInt(ms / 1e3 % 60),
                minutes = parseInt(ms / 6e4 % 60),
                hours = parseInt(ms / 36e5 % 24);
            minutes = hours > 0 && minutes < 10 ? "0" + minutes : minutes, seconds = seconds < 10 ? "0" + seconds : seconds;
            var timeStr = "";
            return hours > 0 && (timeStr = timeStr + hours + ":"), timeStr = timeStr + minutes + ":", timeStr += seconds, $("#boardsize span").text(timeStr), timeStr
        }

        function updateTime() {
            var ms = new Date - time,
                seconds = parseInt(ms / 1e3 % 60),
                minutes = parseInt(ms / 6e4 % 60),
                hours = parseInt(ms / 36e5 % 24);
            timeInSeconds = parseInt(ms / 1e3), minutes = minutes < 10 ? "0" + minutes : minutes, seconds = seconds < 10 ? "0" + seconds : seconds, timeStr = "", minutes += "", seconds += "", hours > 0 && (timeStr = timeStr + hours + ":"), timeStr = timeStr + minutes + ":", timeStr += seconds, $("#minutes-l").text(minutes.split("")[0]), $("#minutes-r").text(minutes.split("")[1]), $("#seconds-l").text(seconds.split("")[0]), $("#seconds-r").text(seconds.split("")[1]), gameEnded || (timerTOH = setTimeout(updateTime, 1e3))
        }

        function endGame() {
            clearTimeout(timerTOH), getScore(function(value) {
                var dailyModeAlreadyCompletedBeforeSoDontGetAnyPoints = !1;
                if (dailyMode) {
                    var progress = Storage.getDataValue("today-progress", {}),
                        progressSize = currentPuzzle.size - 3;
                    progress[progressSize] ? dailyModeAlreadyCompletedBeforeSoDontGetAnyPoints = !0 : (Storage.setDataValue("today-progress", progress), progress[progressSize] = !0, Storage.setDataValue("today-progress", progress)), updateDailyModeCheckmarks()
                }
                var oldScore = 1 * value,
                    newScore = oldScore;
                0 == dailyModeAlreadyCompletedBeforeSoDontGetAnyPoints && (newScore = setScore(grid.width * grid.height, value)), $("#scorenr").html(newScore), continueLastGame = !1, grid.unmark(), grid.hint.hide(), grid.hint.active = !1, Tutorial.end(), systemTilesLockToggleable = !1;
                var ojoo = getOjoo() + "!";
                $("#boardsize").html("<span>" + ojoo + "</span>"), grid.each(function() {
                    this.system = !0
                }), $("#bar [data-action]").hide(), grid.solve(), grid.render(), $("#donate").hide(), 0 == Storage.getDataValue("donated", !1) && Storage.getDataValue("gamesPlayed", 0) >= 5 && $("#donate").show(), Storage.levelCompleted(currentPuzzle.size, newScore, timeInSeconds, currentPuzzle.isTutorial, hintsUsed, undosUsed), endGameTOH3 = setTimeout(function() {
                    $("#grid").addClass("completed"), endGameTOH1 = setTimeout(function() {
                        $("#board").addClass("hidden"), endGameTOH2 = setTimeout(function() {
                            if (gameEnded = !0, $(".menugrid").removeClass("hidden"), $("#chooseSize").addClass("show"), $("#score").show(), 0 == Storage.getDataValue("donated", !1) && Storage.getDataValue("gamesPlayed", 0) >= 5 && $("#donate").show(), PlayCenter.isSignedIn && $('[data-action="achievements"],[data-action="leaderboards"]').show(), startedTutorial ? window.Marker && Marker.save("tutorial", "completed") : (window.Marker && Marker.save("level", "completed", currentPuzzle.size), window.Marker && Marker.save("score", newScore), newScore > oldScore && (animateScore(oldScore, newScore), showAppsIcon && $('[data-action="apps"]').show(), tweet && !currentPuzzle.isTutorial && (updateTweetUrl(currentPuzzle.size), $("#tweeturl").show()), facebook && !currentPuzzle.isTutorial && $("#facebook").show())), currentPuzzle.isTutorial) {
                                var str = dailyMode ? today() : Language.get("Select a size");
                                $("#boardsize").html("<span>" + str + "</span>")
                            }
                            $('#bar [data-action="back"]').show(), $("#time").hide(), setTimeout(function() {
                                $("#score").addClass("show"), $("#grid").removeClass("completed"), setTimeout(function() {
                                    $("#tweeturl").addClass("subtleHintOnce")
                                }, 1e3)
                            }, 0)
                        }, 50)
                    }, 2e3)
                }, 2e3), currentPuzzle.isTutorial || Levels.finishedSize(grid.width)
            })
        }

        function quitCurrentGame() {
            Storage.gameQuitted(), inGame = !1, gameEnded = !0, clearTimeouts(), clearTimeout(timerTOH), $("#time").hide(), grid && (grid.unmark(), grid.hint.hide(), grid.hint.active = !1, grid.each(function() {
                this.system = !0
            })), showSizes()
        }

        function addEventListeners() {
            document.addEventListener("backbutton", backButtonPressed, !1), window.isWebApp || $(document).on(Utils.getEventNames("click"), "#games a, #lovie", function(evt) {
                return evt.preventDefault(), evt.stopPropagation(), evt.stopImmediatePropagation(), !1
            }), window.WinJS && (WinJS.Application.onbackclick = backButtonPressed), $(document).on(Utils.getEventNames("keydown"), function(evt) {
                return 27 == evt.keyCode ? (backButtonPressed(), !1) : 32 == evt.keyCode ? (doAction("help"), !1) : 90 == evt.keyCode && (evt.metaKey || evt.ctrlKey) ? (doAction("undo"), !1) : void 0
            }), $(document).on(Utils.getEventNames("end"), click), $(document).on(Utils.getEventNames("start"), "#grid td", tapTile), $(document).on(Utils.getEventNames("end"), "#hintMsg, #boardsize", function() {
                return Tutorial.active && Tutorial.nextAllowed() ? Tutorial.next() : grid && grid.hint && grid.hint.active && grid.hint.clear(), !1
            }), $(document).on(Utils.getEventNames("contextmenu"), function(e) {
                return e.preventDefault(), e.stopPropagation(), e.stopImmediatePropagation(), !1
            }), $(window).on("orientationchange", resize), tapBoardSizeToSolve && $(document).on("touchend mouseup", "#boardsize", function() {
                grid && grid.solve(), checkForLevelComplete()
            })
        }

        function tapTile(e) {
            if (window.Utils && Utils.isDoubleTapBug(e)) return !1;
            var $el = $(e.target).closest("td"),
                x = 1 * $el.attr("data-x"),
                y = 1 * $el.attr("data-y"),
                tile = grid.tile(x, y),
                rightClick = 3 === e.which;
            if (Tutorial.active && Tutorial.nextAllowed()) return Tutorial.next(), !1;
            if (tile.system) {
                var $tile = $el.find(".tile");
                return $tile.addClass("error"), systemTilesLockShown ? hideSystemTiles() : showSystemTiles(), setTimeout(function() {
                    $tile.removeClass("error")
                }, 500), !1
            }
            if (clearTimeout(checkTOH), Tutorial.active) return Tutorial.tapTile(tile), Tutorial.active || (checkTOH = setTimeout(function() {
                checkForLevelComplete()
            }, 700)), !1;
            grid && grid.hint && grid.hint.clear();
            var lastState, undoState = {
                x: tile.x,
                y: tile.y,
                oldValue: tile.getExportValue(),
                time: new Date
            };
            if (undoStack.length) {
                lastState = undoStack[undoStack.length - 1];
                var lastTile = grid.tile(lastState.x, lastState.y),
                    lastChange = lastState.time;
                (lastTile.id != tile.id || new Date - lastChange > 500) && undoStack.push(undoState)
            } else undoStack.push(undoState);
            return rightClick ? tile.isUnknown() ? tile.wall() : tile.isWall() ? tile.dot() : tile.clear() : tile.isUnknown() ? tile.dot() : tile.isDot() ? tile.wall() : tile.clear(), undoStack.length && ((lastState = undoStack[undoStack.length - 1]).newValue = tile.getExportValue()), updatePercentage(), checkTOH = setTimeout(function() {
                checkForLevelComplete()
            }, 700), !1
        }

        function click(evt) {
            if (window.Utils && Utils.isDoubleTapBug(evt)) return !1;
            var $el = $(evt.target).closest("*[data-action]"),
                action = $(evt.target).closest("*[data-action]").attr("data-action"),
                value = $el.attr("data-value"),
                isLink = !(!$el || !$el.length || "A" != $el[0].nodeName);
            if (!isLink && evt.target && "A" == evt.target.nodeName && (isLink = !0), action && !isLink) return doAction(action, value), !1;
            if (isLink && !window.isWebApp) {
                if ($(evt.target).closest('[data-link="social"]').length) return;
                evt.preventDefault();
                var url = $(evt.target).attr("href");
                return $.browser.android, window.open(url, "_system"), !1
            }
        }

        function doAction(action, value) {
            switch (action) {
                case "toggleHintIcon":
                    toggleHintIcon();
                    break;
                case "show-lovie":
                    showLovie();
                    break;
                case "games":
                    showGames();
                    break;
                case "close-titleScreen":
                    touchSplashScreen();
                    break;
                case "show-menu":
                    clearTimeout(checkTOH), Tutorial.end(), grid && grid.hint.clear(), showMenu(), gameEnded = !0;
                    break;
                case "back":
                    if (inGame && inText) return doAction("show-game");
                    if (gameEnded || Tutorial.active) return doAction("show-menu");
                    clearTimeout(checkTOH), Tutorial.end(), quitCurrentGame(), window.Marker && Marker.save("level", "end", currentPuzzle ? currentPuzzle.size : void 0);
                    break;
                case "next":
                    clearTimeout(checkTOH), Tutorial.end(), grid && grid.hint.clear(), loadGame(lastSize);
                    break;
                case "undo":
                    gameEnded || (window.Marker && Marker.save("button", "undo", currentPuzzle ? currentPuzzle.size : void 0), undo());
                    break;
                case "continue":
                    if (Tutorial.active) return Tutorial.next();
                    continueGame(), window.Marker && Marker.save("button", "continue", currentPuzzle ? currentPuzzle.size : void 0);
                    break;
                case "retry":
                    break;
                case "help":
                    if (gameEnded) break;
                    if (clearTimeout(checkTOH), Tutorial.active && !Tutorial.hintAllowed()) return;
                    grid.hint.visible ? grid.hint.clear() : (grid.hint.clear(), grid.hint.next(), hintsUsed++, window.Marker && Marker.save("button", "hint", currentPuzzle ? currentPuzzle.size : void 0));
                    break;
                case "in-game-about":
                    inGame = !0, showAbout();
                    break;
                case "rules":
                    showRules();
                    break;
                case "thanks":
                    showThanks();
                    break;
                case "show-0hh1":
                    showGames();
                    break;
                case "apps":
                    if (!showAppsIcon) return doAction("show-0hh1");
                    showApps();
                    break;
                case "show-game":
                    inText = !1, showGame();
                    break;
                case "play":
                    dailyMode = !1, tutorialPlayed ? showSizes() : startTutorial();
                    break;
                case "play-daily":
                    dailyMode = !0, tutorialPlayed ? showSizes() : startTutorial();
                    break;
                case "tutorial":
                    startTutorial();
                    break;
                case "about":
                    showAbout();
                    break;
                case "achievements":
                    PlayCenter.showAchievements();
                    break;
                case "leaderboards":
                    PlayCenter.showLeaderboard();
                    break;
                case "playcenter":
                    $.browser.ios && !window.isWebApp && PlayCenter.enabled && !PlayCenter.isSignedIn ? ($("#playcenter").addClass("spin"), PlayCenter.signIn(), clearTimeout(removeSpinTOH), removeSpinTOH = setTimeout(function() {
                        $("#playcenter").removeClass("spin")
                    }, 4e3)) : showOnline();
                    break;
                case "sign-out":
                    PlayCenter.signOut();
                    break;
                case "sign-in":
                    PlayCenter.signIn();
                    break;
                case "stopwatch":
                case "settings":
                    showSettings();
                    break;
                case "color-theme":
                    toggleTheme();
                    break;
                case "toggleTimeTrial":
                    toggleTimeTrial();
                    break;
                case "toggleDonate":
                    purchaseInitiated();
                    break;
                case "afterThanks":
                    if (!thanksShownOnce) return thanksShownOnce = !0, void showMenu();
                    showAbout();
                    break;
                case "contranoid":
                    gotoContranoid();
                    break;
                case "toggleLanguage":
                    Language.toggle(), refreshGameAfterLanguageChange()
            }
        }

        function checkForLevelComplete() {
            grid.emptyTileCount > 0 ? grid.isValid() ? $('#bar [data-action="help"]').removeClass("subtleHint") : hintAboutError() : grid.isValid() ? endGame() : grid.hint.next()
        }

        function hintAboutError() {
            $('#bar [data-action="help"]').removeClass("subtleHint"), clearTimeout(endSubtleHintTOH), setTimeout(function() {
                grid.getClosedWrongTiles().length && ($('#bar [data-action="help"]').addClass("subtleHint"), endSubtleHintTOH = setTimeout(function() {
                    $('#bar [data-action="help"]').removeClass("subtleHint")
                }, 2e3))
            }, 0)
        }

        function checkTutorialPlayed(callback) {
            Storage.getItem("tutorialPlayed", function(resultSet) {
                var played = resultSet.tutorialPlayed + "" == "true";
                tutorialPlayed = played, callback(played)
            })
        }

        function markTutorialAsPlayed() {
            Storage.setItem("tutorialPlayed", !0), tutorialPlayed = !0
        }

        function startTutorial() {
            $("#bar [data-action]").hide(), onHomeScreen = !1, Tutorial.start(), startedTutorial = !0, checkTutorialPlayed(function(played) {
                played || (startedTutorial = !1), markTutorialAsPlayed(), $("#undo").closest(".iconcon").css("display", "none")
            })
        }

        function backButtonPressed() {
            return onHomeScreen ? window.WinJS ? window.close() : navigator.app && navigator.app.exitApp() : doAction("back"), !0
        }

        function getOjoo() {
            var ojoos = Language.get("ojoos");
            return remainingOjoos.length || (remainingOjoos = Utils.shuffle(ojoos.slice(0))), Utils.draw(remainingOjoos)
        }

        function getScore(cb) {
            Storage.getItem("score", function(resultSet) {
                var value = resultSet.score;
                value || (value = 0), cb(value)
            })
        }

        function setScore(addPoints, oldScore) {
            clearTimeout(setScore.TOH);
            var curScore = score = 1 * oldScore,
                newScore = curScore + (addPoints || 0);
            return newScore <= curScore ? curScore : (Storage.setItem("score", newScore), newScore)
        }

        function animateScore(curScore, newScore) {
            function next() {
                $("#scorenr").html(curScore), curScore < newScore && curScore++, setScore.TOH = setTimeout(next, delay)
            }
            var delay = 500 / (newScore - curScore);
            next()
        }

        function undo() {
            if (!undoStack.length) return grid.hint.visible ? (grid.unmark(), void grid.hint.hide()) : void(undone ? grid.hint.show("Nothing to undo.") : grid.hint.show("That's the undo button."));
            var undoState = undoStack.pop(),
                tile = grid.tile(undoState.x, undoState.y),
                value = undoState.oldValue;
            grid.unmark(), value >= 0 ? tile.setExportValue(value) : tile.clear(), tile.mark();
            var s = Language.get("This tile was reversed to ");
            1 == value && (s += Language.get("red.")), 2 == value && (s += Language.get("blue.")), 0 == value && (s += Language.get("its empty state.")), grid.hint.show(s), undone = !0, undosUsed++, updatePercentage(), clearTimeout(checkTOH), checkTOH = setTimeout(function() {
                checkForLevelComplete()
            }, 700)
        }

        function clearTimeouts() {
            clearTimeout(endGameTOH1), clearTimeout(endGameTOH2), clearTimeout(endGameTOH3), clearTimeout(endSubtleHintTOH)
        }

        function updateTweetUrl(size) {
            getScore(function(value) {
                var msgId = dailyMode ? "tweetMessageDaily" : "tweetMessage",
                    msg = Language.get(msgId);
                window.Config && Config.lang && Config.lang[Language.current] && (msg = Config.lang[Language.current][msgId]), msg = (msg = (msg = msg.replace(/\$size/gi, size)).replace(/\$score/gi, value)).replace(/\$today/gi, today().toLowerCase());
                var url = "https://twitter.com/intent/tweet?text=" + encodeURIComponent(msg);
                shareMsg = msg, $("#tweeturl").attr("href", url)
            })
        }

        function continueGame() {
            var oldUndoStack = JSON.parse(JSON.stringify(undoStack));
            startGame(currentPuzzle, !0), $(oldUndoStack).each(function() {
                grid.tile(this.x, this.y).setExportValue(this.newValue)
            }), undoStack = oldUndoStack, setTimeout(function() {
                grid.hint.show(HintType.GameContinued)
            }, 0)
        }

        function showSystemTiles() {
            currentPuzzle.isTutorial || systemTilesLockToggleable && (grid.each(function(x, y, i, t) {
                if (this.system) {
                    var $tile = $("#tile-" + x + "-" + y);
                    this.isWall() && $tile.addClass("system")
                }
            }), systemTilesLockShown = !0, 10 == ++locksToggled && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.happy_lock_toggler))
        }

        function hideSystemTiles() {
            $(".system").removeClass("system"), systemTilesLockShown = !1
        }

        function toggleTheme() {
            theme = Storage.getDataValue("theme", 1), ++theme > 3 && (theme = 1), applyColorTheme(theme)
        }

        function applyColorTheme(theme) {
            $("html").removeClass("theme1 theme2 theme3").addClass("theme" + theme), Storage.setDataValue("theme", theme), window.Themes && Themes.apply(theme)
        }

        function purchaseInitiated() {
            Storage.getDataValue("donated", !1) || (debug ? confirm("Purchase 0h h1 for a $?") && purchaseReceived() : Store.buyFullVersion())
        }

        function purchaseReceived() {
            enableDonatedState(), thanksShownOnce = !1, showThanks()
        }

        function enableDonatedState() {
            Storage.setDataValue("donated", !0), $('[data-action="thanks"]').show(), $('p[data-action="about"]').hide(), $("#donate").hide(), $("#toggleDonateValue").removeClass("link").html(Language.get("Yes")), fullVersion = !0, Storage.data.achievementsUnlocked && Storage.data.achievementsUnlocked.pay_to_win || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.pay_to_win)
        }

        function disableDonation() {
            $("#donate").remove(), $("#toggleDonateValue").remove(), $("#toggleDontae").remove()
        }

        function gotoContranoid() {
            var url;
            url = $.browser.ios ? "https://itunes.apple.com/us/app/contranoid/id1027717534?mt=8" : $.browser.android ? "https://play.google.com/store/apps/details?id=com.q42.contranoid" : $.browser.chromeWebStore ? "https://chrome.google.com/webstore/detail/contranoid/ineojkjjajpfglpmjnndfioncfjkmmdn" : "http://contranoid.com", window.isWebApp ? window.open(url, "_blank") : $.browser.android ? window.open(url, "_system") : $.browser.ios ? window.open(url, "_system") : window.open(url, "_blank")
        }

        function showGames() {
            inText = !0, onHomeScreen = !1, $(".screen").hide().removeClass("show"), $("#games").show(), setTimeout(function() {
                $("#games").addClass("show")
            }, 0), resize()
        }

        function updatePercentage(isEmpty) {
            isEmpty && (updatePercentage.totalTiles = grid.width * grid.height, updatePercentage.initialEmptyTileCount = grid.emptyTileCount, updatePercentage.initialTileCount = updatePercentage.totalTiles - updatePercentage.initialEmptyTileCount);
            var tilesDoneByUser = updatePercentage.totalTiles - grid.emptyTileCount - updatePercentage.initialTileCount,
                percentage = Math.ceil(tilesDoneByUser / updatePercentage.initialEmptyTileCount * 100);
            $("#percentage .value").html(percentage + "%")
        }

        function initLovie() {
            var endDate = new Date(2015, 9, 9, 0, 0, 0);
            new Date > endDate && $("#lovieheart").hide()
        }

        function showLovie() {
            onHomeScreen = !1, inText = !0, $(".screen").hide().removeClass("show"), $("#lovie, #lovie [data-action]").show(), setTimeout(function() {
                $("#lovie").addClass("show")
            }, 0), resize()
        }

        function toggleHintIcon() {
            hideHintIcon = !hideHintIcon, Storage.setDataValue("hideHintIcon", hideHintIcon), updateHintIconState()
        }

        function updateHintIconState() {
            hideHintIcon ? ($("#toggleHintIcon text").text(Language.get("No")), $('[data-action="help"]').addClass("disabled")) : ($("#toggleHintIcon text").text(Language.get("Yes")), $('[data-action="help"]').removeClass("disabled").css("display", "inline-block"))
        }

        function initCordova() {
            cordova.getAppVersion && cordova.getAppVersion.getVersionNumber().then(function(version) {
                $("[data-version]").html("v " + version + " ")
            })
        }

        function today() {
            return Language.get("weekDays")[(new Date).getDay()]
        }

        function refreshGameAfterLanguageChange() {
            remainingOjoos = [];
            var today = Language.get("weekDays")[(new Date).getDay()];
            $("#weekDay").html(today.toLowerCase())
        }

        function getTodayStr() {
            return (new Date).getFullYear() + "-" + ((new Date).getMonth() + 1) + "-" + (new Date).getDate()
        }

        function updateDailyModeCheckmarks() {
            if ($("[data-size]").removeClass("done"), dailyMode) {
                var id = Storage.getDataValue("today-id");
                id && id == getTodayStr() || (Storage.setDataValue("today-id", getTodayStr()), Storage.setDataValue("today-progress", {}));
                var progress = Storage.getDataValue("today-progress", {});
                for (var size in progress) $('[data-size="' + size + '"]').addClass("done")
            }
        }
        var grid, endGameTOH1, endGameTOH2, endGameTOH3, endSubtleHintTOH, removeSpinTOH, debugSize = 0,
            debug = "#debug" == document.location.hash,
            tapBoardSizeToSolve = !1,
            tweet = window.isWebApp,
            facebook = window.isWebApp && !$.browser.chromeWebStore,
            showAppsIcon = window.isWebApp,
            showTime = !1,
            fullVersion = !1,
            dailyMode = !1,
            hideHintIcon = !1,
            startedTutorial = !1,
            tutorialPlayed = !!debug,
            thanksShownOnce = !0,
            splashScreenTouched = 0,
            skipSplash = !1,
            sizesBonus = [4, 5, 6, 7, 8, 9],
            lastSize = 0,
            currentPuzzle = null,
            checkTOH = 0,
            remainingOjoos = [],
            onHomeScreen = !0,
            undoStack = [],
            undone = !1,
            inGame = !1,
            inText = !1,
            gameEnded = !0,
            continueLastGame = !1,
            systemTilesLockShown = !1,
            time = 0,
            timeStr = "",
            timerTOH = 0,
            timeInSeconds = 0,
            hintsUsed = 0,
            undosUsed = 0,
            locksToggled = 0,
            shareMsg = "#0hn0 It's 0h h1's companion! Go get addicted to this lovely puzzle game http://0hn0.com (or get the app)!";
        this.start = function() {
            setTimeout(function() {
                Levels.init()
            }, 100), addEventListeners(), debug ? debugSize ? loadGame(debugSize) : showMenu() : (setTimeout(function() {
                $(".hide0").removeClass("hide0")
            }, 300), setTimeout(function() {
                $(".hide1").removeClass("hide1")
            }, 1300), setTimeout(function() {
                $(".hide-title").removeClass("hide-title")
            }, 2300), setTimeout(function() {
                $(".hide-subtitle").removeClass("hide-subtitle"), skipSplash = !0
            }, 3500))
        }, this.init = function() {
            window.Analytics && Analytics.init();
            var testDebugSize = 1 * document.location.hash.replace(/#/g, "");
            testDebugSize > 0 && (debug = !0, debugSize = testDebugSize), Storage.getDataValue("donated", !1) && enableDonatedState(), window.Themes && Themes.init(), initLovie(), window.cordova && initCordova(), showTime = Storage.getDataValue("showTimeTrial", !1), $("#toggleTimeTrialValue text").text(showTime ? Language.get("Yes") : Language.get("No")), theme = Storage.getDataValue("theme", 1), applyColorTheme(theme), getScore(function(value) {
                $("#scorenr").html(value)
            }), $("#tweeturl, #facebook").hide(), window.isWebApp ? debug || disableDonation() : $("#app, #q42adrow").hide(), Utils.isTouch() ? $("html").addClass("touch") : ($("html").addClass("web"), $(document).on("mousedown touchstart", ".web-link", function(evt) {
                var url = "http://" + $(evt.target).html();
                return window.open(url), !1
            })), $("#menugrid [data-size]").each(function(i, el) {
                var $el = $(el),
                    size = 1 * $el.attr("data-size"),
                    label = sizesBonus[size - 1];
                $el.html(label), $el.on(Utils.getEventNames("start"), function(evt) {
                    if (Utils.isDoubleTapBug(evt)) return !1;
                    loadGame(sizesBonus[1 * $(evt.target).closest("[data-size]").attr("data-size") - 1])
                })
            }), resize(), $(window).on(Utils.getEventNames("resize"), resize), $(window).on(Utils.getEventNames("orientationchange"), resize), Store.init(), showTitleScreen(), resize(), hideHintIcon = Storage.getDataValue("hideHintIcon", !1), updateHintIconState(), Utils.setColorScheme("#ff384b", "#1cc0e0"), window.SocialSharing && addNativeSocialHooks(), window.isWebApp && PlayCenter.enabled && !PlayCenter.isSignedIn && PlayCenter.autoSignIn();
            var config = window.Config || {};
            for (var id in config) config[id] || $('[data-config="' + id + '"]').hide()
        }, this.startGame = startGame, this.showTitleScreen = showTitleScreen, this.showGame = showGame, this.showMenu = showMenu, this.resize = resize, this.showAbout = showAbout, this.showApps = showApps, this.showSettings = showSettings, this.showMessage = function(msg) {
            msg && (onHomeScreen = !1, inText = !0, $(".screen").hide().removeClass("show"), $("#message").show(), $("#message-body").html(msg), setTimeout(function() {
                $("#message").addClass("show")
            }, 0), resize(), window.Marker && Marker.save("page", "message"), clearTimeout(removeSpinTOH), $("#playcenter").removeClass("spin"))
        }, this.showOnline = showOnline, this.show0hh1 = function() {
            onHomeScreen = !1, inText = !0, $(".screen").hide().removeClass("show"), $("#ohhi").show(), setTimeout(function() {
                $("#ohhi").addClass("show")
            }, 0), resize(), window.Marker && Marker.save("page", "0hh1")
        }, this.startTutorial = startTutorial, this.checkForLevelComplete = checkForLevelComplete, this.enableTimer = function() {
            showTime = !0
        }, this.disableTimer = function() {
            showTime = !0
        }, this.undo = undo, this.logo = function(w) {
            w = w || 512, Game.startGame({
                size: 2,
                empty: [4, 3, 0, 1],
                full: [0, 0, 0, 0]
            }), $("body, #container").css("background-color", "transparent"), $("#bar, #boardsize").hide(), $("html, body").css("background", "#000"), $("#board").css("background", "#fff"), $("#container").css("overflow", "auto"), $("#feelsize").css("width", w + "px"), Game.resize(), $("#board").css("height", w + "px"), setTimeout(function() {
                Game.resize(), $("#board").css("height", w + "px"), setTimeout(function() {
                    $("#board #tile-0-1").hide(), $("#board").css("height", w + "px")
                }, 0)
            }, 10)
        }, this.applyColorTheme = applyColorTheme, this.purchaseReceived = purchaseReceived, this.enableDonatedState = enableDonatedState, this.gotoContranoid = gotoContranoid, this.refreshGameAfterLanguageChange = refreshGameAfterLanguageChange, window.__defineGetter__("tile", function() {
            return grid.tile
        }), this.__defineGetter__("grid", function() {
            return grid
        }), this.__defineGetter__("debug", function() {
            return debug
        }), this.__defineGetter__("fullVersion", function() {
            return fullVersion
        })
    },
    TutorialMessages = [{
        msg: "t01",
        tiles: [],
        next: !0
    }, {
        msg: "t02",
        tiles: [],
        next: !0
    }, {
        msg: "t03",
        tiles: [],
        next: !0
    }, {
        msg: "t05",
        tiles: [
            [0, 0]
        ],
        next: !0
    }, {
        msg: "t06",
        tiles: [
            [1, 0, 2],
            [2, 0, 2]
        ]
    }, {
        msg: "t07",
        tiles: [
            [3, 0, 1]
        ]
    }, {
        msg: "t08",
        tiles: [
            [3, 1]
        ],
        next: !0
    }, {
        msg: "t09",
        tiles: [
            [2, 1, 1]
        ]
    }, {
        msg: "t10",
        tiles: [
            [1, 1]
        ],
        next: !0
    }, {
        msg: "t11",
        tiles: [
            [1, 2, 2],
            [1, 3, 2]
        ]
    }, {
        msg: "t12",
        tiles: [
            [0, 2, 1],
            [2, 2, 2],
            [2, 3, 1]
        ]
    }, {
        msg: "",
        tiles: [],
        last: !0
    }],
    Tutorial = new function() {
        function next() {
            for (var i = 0; i < 20; i++) $("html").removeClass("tutorial-" + i);
            if ($("#bar [data-action]").hide(), $('#bar [data-action="back"]').show(), step >= Utils.count(TutorialMessages)) return hide(), active = !1, void setTimeout(function() {
                Game.checkForLevelComplete()
            }, 1e3);
            var t = TutorialMessages[++step];
            msg = Language.get(t.msg), $("html").addClass("tutorial-" + step), show(msg), tilesToTapThisStep = [], Game.grid.unmark(), $(t.tiles).each(function() {
                tilesToTapThisStep.push(Game.grid.tile(this[0], this[1]))
            }), setTimeout(function() {
                markTilesForThisStep()
            }, 0), t.next, t.last && (active = !1)
        }

        function markTilesForThisStep() {
            var t = TutorialMessages[step];
            t.rows ? $(t.rows).each(function() {
                Game.grid.markRow(this)
            }) : t.cols ? $(t.cols).each(function() {
                Game.grid.markCol(this)
            }) : $(t.tiles).each(function() {
                Game.grid.mark(this[0], this[1])
            })
        }

        function show(msg) {
            $("#hintMsg").html("<span>" + msg + "</span>"), $("html").addClass("showHint"), visible = !0
        }

        function hide() {
            $("html").removeClass("showHint"), visible = !1
        }

        function checkStepCompleted() {
            var completed = !0;
            $(TutorialMessages[step].tiles).each(function() {
                var x = this[0],
                    y = this[1],
                    tile = Game.grid.tile(x, y),
                    value = this[2];
                tile.getExportValue() != value ? completed = !1 : setTimeout(function() {
                    tile.unmark(), tile.system = !0
                }, 0)
            }), completed && ($(tilesToTapThisStep).each(function() {
                this.system = !0
            }), next())
        }

        function nextAllowed() {
            var t = TutorialMessages[step];
            return !(!t || !t.next)
        }
        var step = 0,
            active = !1,
            visible = !1,
            tilesToTapThisStep = [];
        this.start = function() {
            $("html").addClass("tutorial"), step = -1, active = !0, Game.startGame({
                size: 4,
                empty: [4, 0, 0, 0, 1, 5, 0, 3, 0, 0, 0, 5, 3, 0, 0, 1],
                full: [4, 2, 2, 1, 1, 5, 1, 3, 1, 2, 2, 5, 3, 2, 1, 1],
                isTutorial: !0
            }), next(), window.Marker && Marker.save("tutorial", "start")
        }, this.end = function() {
            active && window.Marker && Marker.save("tutorial", "end"), $("html").removeClass("tutorial");
            for (var i = 0; i < 20; i++) $("html").removeClass("tutorial-" + i);
            $('#bar [data-action="help"]').removeClass("hidden wiggle"), active = !1
        }, this.next = next, this.show = show, this.hide = hide, this.tapTile = function(tile) {
            TutorialMessages[step];
            if (nextAllowed()) next();
            else {
                var tapAllowed = !1;
                $(tilesToTapThisStep).each(function() {
                    tile.x == this.x && tile.y == this.y && (tapAllowed = !0)
                }), tapAllowed && (tile.isEmpty ? tile.dot() : tile.isDot() ? tile.wall() : tile.clear(), setTimeout(markTilesForThisStep, 0), checkStepCompleted())
            }
        }, this.hintAllowed = function() {
            return step >= 9
        }, this.nextAllowed = nextAllowed, this.__defineGetter__("active", function() {
            return active
        }), this.__defineSetter__("active", function(v) {
            active = v
        }), this.__defineGetter__("visible", function() {
            return visible
        }), this.__defineGetter__("step", function() {
            return step
        })
    };
! function(window, document, undefined) {
    function q(a) {
        return function() {
            return this[a]
        }
    }

    function ba(a, b) {
        var c = a.split("."),
            d = aa;
        !(c[0] in d) && d.execScript && d.execScript("var " + c[0]);
        for (var e; c.length && (e = c.shift());) c.length || b === j ? d = d[e] ? d[e] : d[e] = {} : d[e] = b
    }

    function ca(a, b, c) {
        return a.call.apply(a.bind, arguments)
    }

    function da(a, b, c) {
        if (!a) throw Error();
        if (2 < arguments.length) {
            var d = Array.prototype.slice.call(arguments, 2);
            return function() {
                var c = Array.prototype.slice.call(arguments);
                return Array.prototype.unshift.apply(c, d), a.apply(b, c)
            }
        }
        return function() {
            return a.apply(b, arguments)
        }
    }

    function s(a, b, c) {
        return (s = Function.prototype.bind && -1 != Function.prototype.bind.toString().indexOf("native code") ? ca : da).apply(l, arguments)
    }

    function fa(a, b) {
        this.G = a, this.u = b || a, this.z = this.u.document, this.R = j
    }

    function t(a, b, c) {
        (a = a.z.getElementsByTagName(b)[0]) || (a = document.documentElement), a && a.lastChild && a.insertBefore(c, a.lastChild)
    }

    function u(a, b) {
        return a.createElement("link", {
            rel: "stylesheet",
            href: b
        })
    }

    function ha(a, b) {
        return a.createElement("script", {
            src: b
        })
    }

    function v(a, b) {
        for (var c = a.className.split(/\s+/), d = 0, e = c.length; d < e; d++)
            if (c[d] == b) return;
        c.push(b), a.className = c.join(" ").replace(/\s+/g, " ").replace(/^\s+|\s+$/, "")
    }

    function w(a, b) {
        for (var c = a.className.split(/\s+/), d = [], e = 0, f = c.length; e < f; e++) c[e] != b && d.push(c[e]);
        a.className = d.join(" ").replace(/\s+/g, " ").replace(/^\s+|\s+$/, "")
    }

    function ia(a, b) {
        for (var c = a.className.split(/\s+/), d = 0, e = c.length; d < e; d++)
            if (c[d] == b) return k;
        return p
    }

    function ga(a) {
        if (a.R === j) {
            var b = a.z.createElement("p");
            b.innerHTML = '<a style="top:1px;">w</a>', a.R = /top/.test(b.getElementsByTagName("a")[0].getAttribute("style"))
        }
        return a.R
    }

    function x(a) {
        var b = a.u.location.protocol;
        return "about:" == b && (b = a.G.location.protocol), "https:" == b ? "https:" : "http:"
    }

    function y(a, b, c) {
        this.w = a, this.T = b, this.Aa = c
    }

    function z(a, b, c, d) {
        this.e = a != l ? a : l, this.o = b != l ? b : l, this.ba = c != l ? c : l, this.f = d != l ? d : l
    }

    function A(a) {
        a = ja.exec(a);
        var b = l,
            c = l,
            d = l,
            e = l;
        return a && (a[1] !== l && a[1] && (b = parseInt(a[1], 10)), a[2] !== l && a[2] && (c = parseInt(a[2], 10)), a[3] !== l && a[3] && (d = parseInt(a[3], 10)), a[4] !== l && a[4] && (e = /^[0-9]+$/.test(a[4]) ? parseInt(a[4], 10) : a[4])), new z(b, c, d, e)
    }

    function B(a, b, c, d, e, f, g, h, n, m, r) {
        this.J = a, this.Ha = b, this.za = c, this.ga = d, this.Fa = e, this.fa = f, this.xa = g, this.Ga = h, this.wa = n, this.ea = m, this.k = r
    }

    function C(a, b) {
        this.a = a, this.H = b
    }

    function D(a) {
        var b = F(a.a, /(iPod|iPad|iPhone|Android|Windows Phone|BB\d{2}|BlackBerry)/, 1);
        return "" != b ? (/BB\d{2}/.test(b) && (b = "BlackBerry"), b) : "" != (a = F(a.a, /(Linux|Mac_PowerPC|Macintosh|Windows|CrOS)/, 1)) ? ("Mac_PowerPC" == a && (a = "Macintosh"), a) : "Unknown"
    }

    function E(a) {
        if ((b = F(a.a, /(OS X|Windows NT|Android) ([^;)]+)/, 2)) || (b = F(a.a, /Windows Phone( OS)? ([^;)]+)/, 2)) || (b = F(a.a, /(iPhone )?OS ([\d_]+)/, 2))) return b;
        if (b = F(a.a, /(?:Linux|CrOS) ([^;)]+)/, 1))
            for (var b = b.split(/\s/), c = 0; c < b.length; c += 1)
                if (/^[\d\._]+$/.test(b[c])) return b[c];
        return (a = F(a.a, /(BB\d{2}|BlackBerry).*?Version\/([^\s]*)/, 2)) ? a : "Unknown"
    }

    function F(a, b, c) {
        return (a = a.match(b)) && a[c] ? a[c] : ""
    }

    function G(a) {
        if (a.documentMode) return a.documentMode
    }

    function la(a) {
        this.va = a || "-"
    }

    function H(a, b) {
        this.J = a, this.U = 4, this.K = "n";
        var c = (b || "n4").match(/^([nio])([1-9])$/i);
        c && (this.K = c[1], this.U = parseInt(c[2], 10))
    }

    function I(a) {
        return a.K + a.U
    }

    function ma(a) {
        var b = 4,
            c = "n",
            d = l;
        return a && ((d = a.match(/(normal|oblique|italic)/i)) && d[1] && (c = d[1].substr(0, 1).toLowerCase()), (d = a.match(/([1-9]00|normal|bold)/i)) && d[1] && (/bold/i.test(d[1]) ? b = 7 : /[1-9]00/.test(d[1]) && (b = parseInt(d[1].substr(0, 1), 10)))), c + b
    }

    function na(a, b, c) {
        this.c = a, this.h = b, this.M = c, this.j = "wf", this.g = new la("-")
    }

    function pa(a) {
        v(a.h, a.g.f(a.j, "loading")), J(a, "loading")
    }

    function K(a) {
        w(a.h, a.g.f(a.j, "loading")), ia(a.h, a.g.f(a.j, "active")) || v(a.h, a.g.f(a.j, "inactive")), J(a, "inactive")
    }

    function J(a, b, c) {
        a.M[b] && (c ? a.M[b](c.getName(), I(c)) : a.M[b]())
    }

    function L(a, b) {
        this.c = a, this.C = b, this.s = this.c.createElement("span", {
            "aria-hidden": "true"
        }, this.C)
    }

    function M(a, b) {
        var d, c = a.s;
        d = [];
        for (var e = b.J.split(/,\s*/), f = 0; f < e.length; f++) {
            var g = e[f].replace(/['"]/g, ""); - 1 == g.indexOf(" ") ? d.push(g) : d.push("'" + g + "'")
        }
        d = d.join(","), e = "normal", f = b.U + "00", "o" === b.K ? e = "oblique" : "i" === b.K && (e = "italic"), d = "position:absolute;top:-999px;left:-999px;font-size:300px;width:auto;height:auto;line-height:normal;margin:0;padding:0;font-variant:normal;white-space:nowrap;font-family:" + d + ";font-style:" + e + ";font-weight:" + f + ";", ga(a.c) ? c.setAttribute("style", d) : c.style.cssText = d
    }

    function N(a) {
        t(a.c, "body", a.s)
    }

    function qa(a, b, c, d, e, f, g, h) {
        this.V = a, this.ta = b, this.c = c, this.q = d, this.C = h || "BESbswy", this.k = e, this.F = {}, this.S = f || 5e3, this.Z = g || l, this.B = this.A = l, N(a = new L(this.c, this.C));
        for (var n in O) O.hasOwnProperty(n) && (M(a, new H(O[n], I(this.q))), this.F[O[n]] = a.s.offsetWidth);
        a.remove()
    }

    function sa(a, b, c) {
        for (var d in O)
            if (O.hasOwnProperty(d) && b === a.F[O[d]] && c === a.F[O[d]]) return k;
        return p
    }

    function ra(a) {
        var b = a.A.s.offsetWidth,
            c = a.B.s.offsetWidth;
        b === a.F.serif && c === a.F["sans-serif"] || a.k.T && sa(a, b, c) ? ea() - a.ya >= a.S ? a.k.T && sa(a, b, c) && (a.Z === l || a.Z.hasOwnProperty(a.q.getName())) ? P(a, a.V) : P(a, a.ta) : setTimeout(s(function() {
            ra(this)
        }, a), 25) : P(a, a.V)
    }

    function P(a, b) {
        a.A.remove(), a.B.remove(), b(a.q)
    }

    function R(a, b, c, d) {
        this.c = b, this.t = c, this.N = 0, this.ca = this.Y = p, this.S = d, this.k = a.k
    }

    function ta(a, b, c, d, e) {
        if (0 === b.length && e) K(a.t);
        else
            for (a.N += b.length, e && (a.Y = e), e = 0; e < b.length; e++) {
                var f = b[e],
                    g = c[f.getName()],
                    h = a.t,
                    n = f;
                v(h.h, h.g.f(h.j, n.getName(), I(n).toString(), "loading")), J(h, "fontloading", n), new qa(s(a.ha, a), s(a.ia, a), a.c, f, a.k, a.S, d, g).start()
            }
    }

    function ua(a) {
        0 == --a.N && a.Y && (a.ca ? (a = a.t, w(a.h, a.g.f(a.j, "loading")), w(a.h, a.g.f(a.j, "inactive")), v(a.h, a.g.f(a.j, "active")), J(a, "active")) : K(a.t))
    }

    function S(a, b, c) {
        this.G = a, this.W = b, this.a = c, this.O = this.P = 0
    }

    function T(a, b) {
        U.W.$[a] = b
    }

    function V(a, b) {
        this.c = a, this.d = b
    }

    function W(a, b) {
        this.c = a, this.d = b
    }

    function ya(a) {
        if (a = (b = a.split(":"))[0], b[1]) {
            for (var c = b[1].split(","), b = [], d = 0, e = c.length; d < e; d++) {
                var f = c[d];
                if (f) {
                    var g = xa[f];
                    b.push(g || f)
                }
            }
            for (c = [], d = 0; d < b.length; d += 1) c.push(new H(a, b[d]));
            return c
        }
        return [new H(a)]
    }

    function X(a, b, c) {
        this.a = a, this.c = b, this.d = c, this.m = []
    }

    function Y(a, b) {
        this.c = a, this.d = b, this.m = []
    }

    function za(a, b, c) {
        this.L = a || b + Aa, this.p = [], this.Q = [], this.da = c || ""
    }

    function Ba(a) {
        this.p = a, this.aa = [], this.I = {}
    }

    function Z(a, b, c) {
        this.a = a, this.c = b, this.d = c
    }

    function $(a, b) {
        this.c = a, this.d = b, this.m = []
    }
    var j = void 0,
        k = !0,
        l = null,
        p = !1,
        aa = this;
    aa.Ba = k;
    var ea = Date.now || function() {
        return +new Date
    };
    fa.prototype.createElement = function(a, b, c) {
        if (a = this.z.createElement(a), b)
            for (var d in b)
                if (b.hasOwnProperty(d))
                    if ("style" == d) {
                        var e = a,
                            f = b[d];
                        ga(this) ? e.setAttribute("style", f) : e.style.cssText = f
                    } else a.setAttribute(d, b[d]);
        return c && a.appendChild(this.z.createTextNode(c)), a
    }, ba("webfont.BrowserInfo", y), y.prototype.qa = q("w"), y.prototype.hasWebFontSupport = y.prototype.qa, y.prototype.ra = q("T"), y.prototype.hasWebKitFallbackBug = y.prototype.ra, y.prototype.sa = q("Aa"), y.prototype.hasWebKitMetricsBug = y.prototype.sa;
    var ja = /^([0-9]+)(?:[\._-]([0-9]+))?(?:[\._-]([0-9]+))?(?:[\._+-]?(.*))?$/;
    z.prototype.toString = function() {
        return [this.e, this.o || "", this.ba || "", this.f || ""].join("")
    }, ba("webfont.UserAgent", B), B.prototype.getName = q("J"), B.prototype.getName = B.prototype.getName, B.prototype.pa = q("za"), B.prototype.getVersion = B.prototype.pa, B.prototype.la = q("ga"), B.prototype.getEngine = B.prototype.la, B.prototype.ma = q("fa"), B.prototype.getEngineVersion = B.prototype.ma, B.prototype.na = q("xa"), B.prototype.getPlatform = B.prototype.na, B.prototype.oa = q("wa"), B.prototype.getPlatformVersion = B.prototype.oa, B.prototype.ka = q("ea"), B.prototype.getDocumentMode = B.prototype.ka, B.prototype.ja = q("k"), B.prototype.getBrowserInfo = B.prototype.ja;
    var ka = new B("Unknown", new z, "Unknown", "Unknown", new z, "Unknown", "Unknown", new z, "Unknown", j, new y(p, p, p));
    C.prototype.parse = function() {
        var a;
        if (-1 != this.a.indexOf("MSIE")) {
            a = D(this);
            c = A(b = E(this));
            a = new B("MSIE", e = A(d = F(this.a, /MSIE ([\d\w\.]+)/, 1)), d, "MSIE", e, d, a, c, b, G(this.H), new y("Windows" == a && 6 <= e.e || "Windows Phone" == a && 8 <= c.e, p, p))
        } else if (-1 != this.a.indexOf("Opera")) a: {
            a = "Unknown";
            var c = A(b = F(this.a, /Presto\/([\d\w\.]+)/, 1)),
                e = A(d = E(this)),
                f = G(this.H);
            if (c.e !== l ? a = "Presto" : (-1 != this.a.indexOf("Gecko") && (a = "Gecko"), b = F(this.a, /rv:([^\)]+)/, 1), c = A(b)), -1 != this.a.indexOf("Opera Mini/")) a = new B("OperaMini", h = A(g = F(this.a, /Opera Mini\/([\d\.]+)/, 1)), g, a, c, b, D(this), e, d, f, new y(p, p, p));
            else {
                if (-1 != this.a.indexOf("Version/") && (g = F(this.a, /Version\/([\d\.]+)/, 1), (h = A(g)).e !== l)) {
                    a = new B("Opera", h, g, a, c, b, D(this), e, d, f, new y(10 <= h.e, p, p));
                    break a
                }
                a = (h = A(g = F(this.a, /Opera[\/ ]([\d\.]+)/, 1))).e !== l ? new B("Opera", h, g, a, c, b, D(this), e, d, f, new y(10 <= h.e, p, p)) : new B("Opera", new z, "Unknown", a, c, b, D(this), e, d, f, new y(p, p, p))
            }
        }
        else if (/AppleWeb(K|k)it/.test(this.a)) {
            a = D(this);
            var b = E(this),
                c = A(b),
                d = F(this.a, /AppleWeb(?:K|k)it\/([\d\.\+]+)/, 1),
                e = A(d),
                f = "Unknown",
                g = new z,
                h = "Unknown",
                n = p; - 1 != this.a.indexOf("Chrome") || -1 != this.a.indexOf("CrMo") || -1 != this.a.indexOf("CriOS") ? f = "Chrome" : /Silk\/\d/.test(this.a) ? f = "Silk" : "BlackBerry" == a || "Android" == a ? f = "BuiltinBrowser" : -1 != this.a.indexOf("Safari") ? f = "Safari" : -1 != this.a.indexOf("AdobeAIR") && (f = "AdobeAIR"), "BuiltinBrowser" == f ? h = "Unknown" : "Silk" == f ? h = F(this.a, /Silk\/([\d\._]+)/, 1) : "Chrome" == f ? h = F(this.a, /(Chrome|CrMo|CriOS)\/([\d\.]+)/, 2) : -1 != this.a.indexOf("Version/") ? h = F(this.a, /Version\/([\d\.\w]+)/, 1) : "AdobeAIR" == f && (h = F(this.a, /AdobeAIR\/([\d\.]+)/, 1)), g = A(h), n = "AdobeAIR" == f ? 2 < g.e || 2 == g.e && 5 <= g.o : "BlackBerry" == a ? 10 <= c.e : "Android" == a ? 2 < c.e || 2 == c.e && 1 < c.o : 526 <= e.e || 525 <= e.e && 13 <= e.o, a = new B(f, g, h, "AppleWebKit", e, d, a, c, b, G(this.H), new y(n, 536 > e.e || 536 == e.e && 11 > e.o, "iPhone" == a || "iPad" == a || "iPod" == a || "Macintosh" == a))
        } else -1 != this.a.indexOf("Gecko") ? (a = "Unknown", b = new z, c = "Unknown", d = E(this), e = A(d), f = p, -1 != this.a.indexOf("Firefox") ? (a = "Firefox", c = F(this.a, /Firefox\/([\d\w\.]+)/, 1), b = A(c), f = 3 <= b.e && 5 <= b.o) : -1 != this.a.indexOf("Mozilla") && (a = "Mozilla"), g = F(this.a, /rv:([^\)]+)/, 1), h = A(g), f || (f = 1 < h.e || 1 == h.e && 9 < h.o || 1 == h.e && 9 == h.o && 2 <= h.ba || g.match(/1\.9\.1b[123]/) != l || g.match(/1\.9\.1\.[\d\.]+/) != l), a = new B(a, b, c, "Gecko", h, g, D(this), e, d, G(this.H), new y(f, p, p))) : a = ka;
        return a
    }, la.prototype.f = function(a) {
        for (var b = [], c = 0; c < arguments.length; c++) b.push(arguments[c].replace(/[\W_]+/g, "").toLowerCase());
        return b.join(this.va)
    }, H.prototype.getName = q("J"), L.prototype.remove = function() {
        var a = this.s;
        a.parentNode && a.parentNode.removeChild(a)
    };
    var O = {
        Ea: "serif",
        Da: "sans-serif",
        Ca: "monospace"
    };
    qa.prototype.start = function() {
        this.A = new L(this.c, this.C), N(this.A), this.B = new L(this.c, this.C), N(this.B), this.ya = ea(), M(this.A, new H(this.q.getName() + ",serif", I(this.q))), M(this.B, new H(this.q.getName() + ",sans-serif", I(this.q))), ra(this)
    }, R.prototype.ha = function(a) {
        var b = this.t;
        w(b.h, b.g.f(b.j, a.getName(), I(a).toString(), "loading")), w(b.h, b.g.f(b.j, a.getName(), I(a).toString(), "inactive")), v(b.h, b.g.f(b.j, a.getName(), I(a).toString(), "active")), J(b, "fontactive", a), this.ca = k, ua(this)
    }, R.prototype.ia = function(a) {
        var b = this.t;
        w(b.h, b.g.f(b.j, a.getName(), I(a).toString(), "loading")), ia(b.h, b.g.f(b.j, a.getName(), I(a).toString(), "active")) || v(b.h, b.g.f(b.j, a.getName(), I(a).toString(), "inactive")), J(b, "fontinactive", a), ua(this)
    }, S.prototype.load = function(a) {
        var b = a.context || this.G;
        if (this.c = new fa(this.G, b), b = new na(this.c, b.document.documentElement, a), this.a.k.w) {
            var f, c = this.W,
                d = this.c,
                e = [];
            for (f in a)
                if (a.hasOwnProperty(f)) {
                    var g = c.$[f];
                    g && e.push(g(a[f], d))
                } for (a = a.timeout, this.O = this.P = e.length, a = new R(this.a, this.c, b, a), f = 0, c = e.length; f < c; f++)(d = e[f]).v(this.a, s(this.ua, this, d, b, a))
        } else K(b)
    }, S.prototype.ua = function(a, b, c, d) {
        var e = this;
        d ? a.load(function(a, d, h) {
            var n = 0 == --e.P;
            n && pa(b), setTimeout(function() {
                ta(c, a, d || {}, h || l, n)
            }, 0)
        }) : (a = 0 == --this.P, this.O--, a && (0 == this.O ? K(b) : pa(b)), ta(c, [], {}, l, a))
    };
    var va = window,
        wa = new C(navigator.userAgent, document).parse(),
        U = va.WebFont = new S(window, new function() {
            this.$ = {}
        }, wa);
    U.load = U.load, V.prototype.load = function(a) {
        var b, c, d = this.d.urls || [],
            e = this.d.families || [];
        for (b = 0, c = d.length; b < c; b++) t(this.c, "head", u(this.c, d[b]));
        for (d = [], b = 0, c = e.length; b < c; b++) {
            var f = e[b].split(":");
            if (f[1])
                for (var g = f[1].split(","), h = 0; h < g.length; h += 1) d.push(new H(f[0], g[h]));
            else d.push(new H(f[0]))
        }
        a(d)
    }, V.prototype.v = function(a, b) {
        return b(a.k.w)
    }, T("custom", function(a, b) {
        return new V(b, a)
    });
    var xa = {
        regular: "n4",
        bold: "n7",
        italic: "i4",
        bolditalic: "i7",
        r: "n4",
        b: "n7",
        i: "i4",
        bi: "i7"
    };
    W.prototype.v = function(a, b) {
        return b(a.k.w)
    }, W.prototype.load = function(a) {
        t(this.c, "head", u(this.c, x(this.c) + "//webfonts.fontslive.com/css/" + this.d.key + ".css"));
        for (var b = this.d.families, c = [], d = 0, e = b.length; d < e; d++) c.push.apply(c, ya(b[d]));
        a(c)
    }, T("ascender", function(a, b) {
        return new W(b, a)
    }), X.prototype.v = function(a, b) {
        var c = this,
            d = c.d.projectId,
            e = c.d.version;
        if (d) {
            var f = c.c.u,
                g = c.c.createElement("script");
            g.id = "__MonotypeAPIScript__" + d;
            var h = p;
            g.onload = g.onreadystatechange = function() {
                if (!(h || this.readyState && "loaded" !== this.readyState && "complete" !== this.readyState)) {
                    if (h = k, f["__mti_fntLst" + d]) {
                        var e = f["__mti_fntLst" + d]();
                        if (e)
                            for (var m = 0; m < e.length; m++) c.m.push(new H(e[m].fontfamily))
                    }
                    b(a.k.w), g.onload = g.onreadystatechange = l
                }
            }, g.src = c.D(d, e), t(this.c, "head", g)
        } else b(k)
    }, X.prototype.D = function(a, b) {
        return x(this.c) + "//" + (this.d.api || "fast.fonts.com/jsapi").replace(/^.*http(s?):(\/\/)?/, "") + "/" + a + ".js" + (b ? "?v=" + b : "")
    }, X.prototype.load = function(a) {
        a(this.m)
    }, T("monotype", function(a, b) {
        return new X(new C(navigator.userAgent, document).parse(), b, a)
    }), Y.prototype.D = function(a) {
        var b = x(this.c);
        return (this.d.api || b + "//use.typekit.net") + "/" + a + ".js"
    }, Y.prototype.v = function(a, b) {
        var c = this.d.id,
            d = this.d,
            e = this.c.u,
            f = this;
        c ? (e.__webfonttypekitmodule__ || (e.__webfonttypekitmodule__ = {}), e.__webfonttypekitmodule__[c] = function(c) {
            c(a, d, function(a, c, d) {
                for (var e = 0; e < c.length; e += 1) {
                    var g = d[c[e]];
                    if (g)
                        for (var Q = 0; Q < g.length; Q += 1) f.m.push(new H(c[e], g[Q]));
                    else f.m.push(new H(c[e]))
                }
                b(a)
            })
        }, c = ha(this.c, this.D(c)), t(this.c, "head", c)) : b(k)
    }, Y.prototype.load = function(a) {
        a(this.m)
    }, T("typekit", function(a, b) {
        return new Y(b, a)
    });
    var Aa = "//fonts.googleapis.com/css";
    za.prototype.f = function() {
        if (0 == this.p.length) throw Error("No fonts to load !");
        if (-1 != this.L.indexOf("kit=")) return this.L;
        for (var a = this.p.length, b = [], c = 0; c < a; c++) b.push(this.p[c].replace(/ /g, "+"));
        return a = this.L + "?family=" + b.join("%7C"), 0 < this.Q.length && (a += "&subset=" + this.Q.join(",")), 0 < this.da.length && (a += "&text=" + encodeURIComponent(this.da)), a
    };
    var Ca = {
            latin: "BESbswy",
            cyrillic: "&#1081;&#1103;&#1046;",
            greek: "&#945;&#946;&#931;",
            khmer: "&#x1780;&#x1781;&#x1782;",
            Hanuman: "&#x1780;&#x1781;&#x1782;"
        },
        Da = {
            thin: "1",
            extralight: "2",
            "extra-light": "2",
            ultralight: "2",
            "ultra-light": "2",
            light: "3",
            regular: "4",
            book: "4",
            medium: "5",
            "semi-bold": "6",
            semibold: "6",
            "demi-bold": "6",
            demibold: "6",
            bold: "7",
            "extra-bold": "8",
            extrabold: "8",
            "ultra-bold": "8",
            ultrabold: "8",
            black: "9",
            heavy: "9",
            l: "3",
            r: "4",
            b: "7"
        },
        Ea = {
            i: "i",
            italic: "i",
            n: "n",
            normal: "n"
        },
        Fa = RegExp("^(thin|(?:(?:extra|ultra)-?)?light|regular|book|medium|(?:(?:semi|demi|extra|ultra)-?)?bold|black|heavy|l|r|b|[1-9]00)?(n|i|normal|italic)?$");
    Ba.prototype.parse = function() {
        for (var a = this.p.length, b = 0; b < a; b++) {
            var c = this.p[b].split(":"),
                d = c[0].replace(/\+/g, " "),
                e = ["n4"];
            if (2 <= c.length) {
                var f;
                if (f = [], g = c[1])
                    for (var g = g.split(","), h = g.length, n = 0; n < h; n++) {
                        var m;
                        if ((m = g[n]).match(/^[\w]+$/)) {
                            m = Fa.exec(m.toLowerCase());
                            r = j;
                            if (m == l) r = "";
                            else {
                                if (r = j, (r = m[1]) == l || "" == r) r = "4";
                                else var oa = Da[r],
                                    r = oa || (isNaN(r) ? "4" : r.substr(0, 1));
                                r = [m[2] == l || "" == m[2] ? "n" : Ea[m[2]], r].join("")
                            }
                            m = r
                        } else m = "";
                        m && f.push(m)
                    }
                0 < f.length && (e = f), 3 == c.length && (c = c[2], f = [], 0 < (c = c ? c.split(",") : f).length && (c = Ca[c[0]]) && (this.I[d] = c))
            }
            for (this.I[d] || (c = Ca[d]) && (this.I[d] = c), c = 0; c < e.length; c += 1) this.aa.push(new H(d, e[c]))
        }
    };
    var Ga = {
        Arimo: k,
        Cousine: k,
        Tinos: k
    };
    Z.prototype.v = function(a, b) {
        b(a.k.w)
    }, Z.prototype.load = function(a) {
        var b = this.c;
        if ("MSIE" == this.a.getName() && this.d.blocking != k) {
            var c = s(this.X, this, a),
                d = function() {
                    b.z.body ? c() : setTimeout(d, 0)
                };
            d()
        } else this.X(a)
    }, Z.prototype.X = function(a) {
        for (var b = this.c, c = new za(this.d.api, x(b), this.d.text), d = this.d.families, e = d.length, f = 0; f < e; f++) {
            var g = d[f].split(":");
            3 == g.length && c.Q.push(g.pop());
            var h = "";
            2 == g.length && "" != g[1] && (h = ":"), c.p.push(g.join(h))
        }(d = new Ba(d)).parse(), t(b, "head", u(b, c.f())), a(d.aa, d.I, Ga)
    }, T("google", function(a, b) {
        return new Z(new C(navigator.userAgent, document).parse(), b, a)
    }), $.prototype.D = function(a) {
        return x(this.c) + (this.d.api || "//f.fontdeck.com/s/css/js/") + (this.c.u.location.hostname || this.c.G.location.hostname) + "/" + a + ".js"
    }, $.prototype.v = function(a, b) {
        var c = this.d.id,
            d = this.c.u,
            e = this;
        c ? (d.__webfontfontdeckmodule__ || (d.__webfontfontdeckmodule__ = {}), d.__webfontfontdeckmodule__[c] = function(a, c) {
            for (var d = 0, n = c.fonts.length; d < n; ++d) {
                var m = c.fonts[d];
                e.m.push(new H(m.name, ma("font-weight:" + m.weight + ";font-style:" + m.style)))
            }
            b(a)
        }, c = ha(this.c, this.D(c)), t(this.c, "head", c)) : b(k)
    }, $.prototype.load = function(a) {
        a(this.m)
    }, T("fontdeck", function(a, b) {
        return new $(b, a)
    }), window.WebFontConfig && U.load(window.WebFontConfig)
}(this, document);
var Levels = new function() {
        function loadFromStorage() {
            try {
                var loadedPuzzles = JSON.parse(Storage.getDataValue("puzzles", JSON.stringify(puzzles)));
                loadedPuzzles.size5 && (puzzles = loadedPuzzles), puzzles.size4 || (puzzles.size4 = [], puzzles.size9 = [])
            } catch (e) {}
        }

        function saveToStorage() {
            Storage.setDataValue("puzzles", JSON.stringify(puzzles))
        }

        function create(size) {
            for (var puzzle, grid = null, attempts = 0; attempts++ < 10;) {
                grid = new Grid(size), puzzle = {
                    size: size,
                    full: [],
                    empty: [],
                    quality: 0,
                    ms: 0
                };
                var d = new Date;
                if (grid.clear(), grid.generate(size), grid.maxify(size), Game.debug && console.log("attempt", attempts, "valid", grid.isValid(!0)), grid.isValid(!0)) {
                    puzzle.full = grid.getValues(), grid.breakDown(), puzzle.empty = grid.getValues(), puzzle.ms = new Date - d, puzzle.quality = grid.quality;
                    break
                }
                Game.debug && console.warn("INVALID!", grid), grid = null
            }
            return Game.debug && console.log("attempts, ", attempts), Game.debug && console.log(puzzle.ms, puzzle), puzzle
        }
        var puzzles = {
                size4: [],
                size5: [],
                size6: [],
                size7: [],
                size8: [],
                size9: []
            },
            qualityThreshold = {
                4: 60,
                5: 60,
                6: 60,
                7: 60,
                8: 60,
                9: 60
            };
        this.hasPuzzleAvailable = function(size) {
            var puzzleArr = puzzles["size" + size];
            return !(!puzzleArr || !puzzleArr.length)
        }, this.finishedSize = function(size) {
            var puzzleArr = puzzles["size" + size];
            puzzleArr && puzzleArr.length && (puzzleArr.shift(), saveToStorage(), BackgroundService.kick())
        }, this.addSize = function(size, puzzle) {
            var puzzleArr = puzzles["size" + size];
            if (!puzzleArr) return !1;
            puzzleArr.push(puzzle), saveToStorage(), BackgroundService.kick()
        }, this.getSize = function(size, forceGenerated) {
            var puzzleArr = puzzles["size" + size];
            if (forceGenerated || !puzzleArr || !puzzleArr.length) return create(size);
            var puzzle = puzzleArr[0];
            return puzzleArr.length > 1 && (puzzleArr.shift(), saveToStorage(), BackgroundService.kick()), puzzle
        }, this.create = create, this.needs = function() {
            for (var checkForLength = 1; checkForLength <= 2; checkForLength++)
                for (var size in qualityThreshold) {
                    var arr = puzzles["size" + (size *= 1)];
                    if (arr && arr.length < checkForLength) return size
                }
            return !1
        }, this.init = function() {
            loadFromStorage(), BackgroundService.kick()
        }, this.__defineGetter__("puzzles", function() {
            return puzzles
        })
    },
    FixedLevels = new function() {
        var emptyPuzzles = {
            size4: [{
                size: 4,
                full: [3, 4, 1, 4, 1, 5, 6, 6, 3, 1, 5, 5, 3, 1, 4, 1],
                empty: [0, 4, 0, 0, 1, 0, 6, 0, 0, 0, 5, 5, 3, 0, 4, 0],
                quality: 56,
                ms: 26
            }, {
                size: 4,
                full: [4, 5, 6, 1, 1, 5, 6, 6, 3, 1, 5, 5, 4, 3, 1, 4],
                empty: [0, 5, 6, 0, 1, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 4],
                quality: 69,
                ms: 26
            }],
            size5: [{
                size: 5,
                full: [5, 4, 1, 5, 3, 7, 6, 5, 7, 1, 4, 1, 1, 4, 1, 1, 4, 4, 1, 1, 6, 7, 7, 6, 6],
                empty: [5, 0, 0, 0, 3, 0, 0, 5, 0, 1, 0, 0, 0, 4, 0, 0, 4, 0, 0, 1, 0, 7, 7, 6, 0],
                quality: 60,
                ms: 26
            }, {
                size: 5,
                full: [4, 3, 1, 4, 5, 3, 1, 4, 5, 6, 1, 4, 1, 1, 4, 6, 7, 5, 5, 1, 4, 5, 1, 1, 1],
                empty: [0, 3, 0, 0, 5, 3, 0, 0, 5, 0, 0, 4, 0, 0, 4, 0, 7, 0, 0, 0, 4, 0, 1, 1, 0],
                quality: 60,
                ms: 16
            }],
            size6: [{
                size: 6,
                full: [1, 5, 8, 6, 7, 1, 1, 1, 8, 6, 7, 7, 1, 4, 6, 1, 5, 5, 6, 6, 8, 7, 1, 4, 3, 1, 1, 4, 1, 1, 1, 6, 6, 8, 6, 6],
                empty: [0, 5, 8, 6, 7, 0, 0, 0, 0, 0, 7, 0, 0, 4, 0, 0, 5, 0, 0, 6, 0, 0, 1, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 8, 6, 0],
                quality: 64,
                ms: 54
            }, {
                size: 6,
                full: [4, 5, 5, 1, 4, 3, 1, 6, 6, 7, 6, 1, 1, 1, 1, 4, 1, 1, 1, 1, 6, 6, 4, 1, 1, 4, 5, 1, 1, 1, 1, 4, 5, 1, 1, 1],
                empty: [0, 5, 5, 1, 4, 3, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 6, 0, 1, 1, 0, 0, 1, 1, 0, 1, 4, 5, 1, 0, 1],
                quality: 53,
                ms: 46
            }],
            size7: [{
                size: 7,
                full: [3, 5, 1, 4, 4, 1, 4, 1, 7, 6, 6, 6, 1, 4, 6, 6, 5, 1, 1, 7, 5, 4, 1, 1, 4, 4, 8, 1, 6, 4, 6, 1, 1, 6, 1, 1, 1, 7, 6, 5, 9, 1, 5, 5, 7, 6, 1, 6, 1],
                empty: [0, 5, 1, 0, 4, 0, 0, 0, 7, 0, 6, 6, 1, 4, 0, 0, 5, 0, 1, 0, 0, 0, 0, 1, 4, 0, 0, 0, 6, 0, 0, 0, 0, 6, 0, 1, 0, 0, 6, 0, 9, 1, 5, 0, 0, 0, 0, 0, 1],
                quality: 59,
                ms: 234
            }, {
                size: 7,
                full: [4, 1, 1, 1, 7, 7, 5, 6, 5, 4, 1, 7, 7, 5, 5, 4, 1, 7, 7, 7, 1, 1, 1, 7, 9, 9, 9, 6, 5, 7, 6, 8, 1, 1, 1, 1, 4, 1, 8, 6, 6, 6, 4, 6, 4, 1, 5, 5, 5],
                empty: [0, 1, 1, 0, 0, 7, 0, 0, 5, 0, 0, 7, 7, 5, 5, 0, 0, 7, 0, 0, 0, 0, 1, 0, 0, 0, 9, 0, 5, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 0, 0, 0, 0, 4, 0, 0, 5, 5],
                quality: 65,
                ms: 155
            }],
            size8: [{
                size: 8,
                full: [4, 4, 1, 9, 6, 8, 8, 7, 6, 6, 8, 8, 1, 6, 6, 5, 1, 1, 9, 9, 6, 8, 8, 1, 9, 8, 8, 8, 1, 1, 1, 6, 8, 7, 7, 1, 8, 8, 6, 9, 7, 6, 1, 8, 9, 9, 7, 10, 7, 6, 1, 6, 7, 7, 1, 6, 6, 1, 1, 8, 9, 9, 6, 10],
                empty: [4, 4, 0, 9, 0, 8, 0, 7, 0, 0, 8, 0, 0, 6, 6, 0, 0, 0, 0, 0, 0, 0, 8, 0, 0, 0, 0, 8, 0, 1, 0, 6, 8, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 8, 0, 9, 7, 0, 0, 6, 0, 0, 0, 7, 0, 0, 0, 0, 1, 0, 9, 0, 0, 0],
                quality: 67,
                ms: 277
            }, {
                size: 8,
                full: [4, 5, 9, 1, 5, 6, 5, 8, 1, 5, 9, 5, 1, 3, 1, 5, 5, 1, 9, 5, 7, 1, 1, 5, 7, 8, 9, 1, 8, 7, 9, 8, 7, 8, 9, 1, 7, 6, 8, 1, 7, 8, 9, 1, 8, 7, 9, 7, 1, 6, 1, 3, 1, 1, 7, 5, 6, 10, 6, 7, 6, 1, 7, 5],
                empty: [4, 0, 0, 0, 5, 6, 0, 8, 0, 0, 9, 0, 0, 0, 1, 0, 0, 0, 0, 0, 7, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 7, 0, 0, 0, 0, 6, 8, 1, 7, 8, 0, 0, 0, 0, 0, 0, 0, 6, 0, 3, 0, 0, 7, 0, 0, 0, 6, 0, 0, 0, 0, 5],
                quality: 70,
                ms: 417
            }],
            size9: [{
                size: 9,
                full: [1, 10, 7, 10, 7, 8, 8, 1, 4, 3, 6, 1, 5, 1, 6, 6, 9, 7, 1, 8, 9, 8, 8, 1, 1, 7, 5, 8, 10, 11, 10, 10, 11, 1, 6, 1, 3, 1, 6, 1, 9, 10, 10, 10, 10, 1, 6, 7, 1, 9, 10, 10, 10, 10, 7, 8, 9, 7, 1, 7, 7, 1, 6, 5, 6, 1, 4, 1, 9, 9, 6, 9, 8, 9, 6, 8, 6, 1, 8, 5, 8],
                empty: [0, 0, 0, 0, 0, 8, 0, 0, 0, 3, 0, 0, 5, 0, 0, 0, 0, 7, 1, 0, 0, 8, 0, 0, 1, 0, 0, 8, 10, 11, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 10, 0, 0, 0, 1, 0, 7, 0, 9, 10, 0, 10, 10, 0, 0, 0, 0, 0, 0, 7, 0, 6, 0, 6, 0, 0, 1, 0, 0, 6, 0, 8, 0, 0, 8, 0, 0, 0, 0, 0],
                quality: 68,
                ms: 733
            }, {
                size: 9,
                full: [10, 7, 7, 8, 7, 9, 1, 6, 6, 5, 1, 1, 3, 1, 7, 9, 8, 8, 5, 1, 1, 1, 8, 8, 10, 9, 9, 6, 7, 1, 5, 5, 1, 8, 7, 7, 1, 6, 1, 7, 7, 6, 9, 1, 1, 6, 9, 5, 7, 1, 6, 9, 6, 8, 4, 7, 1, 1, 1, 1, 1, 4, 6, 1, 11, 8, 8, 8, 8, 8, 1, 5, 1, 1, 9, 9, 9, 9, 9, 8, 11],
                empty: [10, 7, 7, 8, 0, 0, 0, 6, 6, 0, 0, 0, 0, 0, 0, 9, 8, 0, 5, 0, 0, 0, 0, 8, 0, 0, 0, 6, 7, 0, 5, 0, 1, 0, 0, 7, 0, 0, 0, 0, 7, 0, 0, 0, 0, 6, 0, 5, 0, 0, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 4, 0, 0, 0, 8, 0, 8, 0, 0, 1, 0, 0, 0, 9, 0, 0, 0, 0, 8, 11],
                quality: 67,
                ms: 578
            }]
        };
        this.has = function(size) {
            var id = "fixedLevelSize" + size;
            return alreadyServed = 1 * Storage.getDataValue(id, 0), pool = emptyPuzzles["size" + size], !!(pool && alreadyServed < pool.length)
        }, this.get = function(size) {
            var id = "fixedLevelSize" + size;
            if (alreadyServed = 1 * Storage.getDataValue(id, 0), pool = emptyPuzzles["size" + size], Game.debug && console.log("retrieving fixed level " + size + ", ", alreadyServed), pool && alreadyServed < pool.length) {
                var puzzle = pool[alreadyServed];
                return Storage.setDataValue(id, alreadyServed + 1), puzzle
            }
            return null
        }
    },
    Store = new function() {
        function reloadLicense() {
            licenseInformation.isActive && (licenseInformation.isTrial || Game.purchaseReceived())
        }

        function registerProducts() {
            store.register({
                id: fullVersionId,
                alias: fullVersionId,
                type: store.NON_CONSUMABLE
            })
        }

        function storeIsReady() {
            enabled = !0, registerPurchaseFlow(), Game.fullVersion || restorePurchases(), debug && alert("\\o/ STORE READY \\o/")
        }

        function registerPurchaseFlow() {
            enabled && (store.when(fullVersionId).owned(function(product) {
                debug && alert("FULL VERSION OWNED, UNLOCKING"), Game.enableDonatedState()
            }), store.when(fullVersionId).approved(function(product) {
                debug && alert("FULL VERSION APPROVED, UNLOCKING"), product.finish(), Game.purchaseReceived()
            }), store.when(fullVersionId).cancelled(function(product) {
                debug && alert("FULL VERSION CANCELLED")
            }), store.when(fullVersionId).error(function(product) {
                debug && alert("FULL VERSION ERROR")
            }), window.MSApp && reloadLicense())
        }

        function restorePurchases(manually) {
            if (enabled && !Game.fullVersion) {
                if (debug && alert("RESTORE PURCHASE?"), window.store) {
                    var p = store.get(fullVersionId);
                    if (p && p.transaction && p.transaction.id) debug && alert("RESTORING PURCHASE BY TRANSACTION"), manually ? Game.purchaseReceived() : Game.enableDonatedState();
                    else if (manually) {
                        var msgObj = {
                            action: "back",
                            text: "<h1>" + lang("couldNotRestorePurchase") + "</h1>"
                        };
                        Game.showMessages(msgObj)
                    }
                    debug && alert(JSON.stringify(p))
                }
                window.MSApp && reloadLicense()
            }
        }
        var enabled = !1,
            debug = !1,
            fullVersionId = "0hn0_supporter",
            licenseInformation = null;
        this.init = function() {
            window.store && (store.verbosity = store.DEBUG, registerProducts(), store.ready(storeIsReady), store.refresh(), debug && store.error(function(e) {
                alert("STORE ERROR " + e.code + ": " + e.message)
            })), window.MSApp && (currentApp = Windows.ApplicationModel.Store.CurrentApp, licenseInformation = currentApp.licenseInformation, enabled = !0, licenseInformation.addEventListener("licensechanged", reloadLicense))
        }, this.buyFullVersion = function() {
            if (enabled && (window.MSApp || store.order(fullVersionId), window.MSApp)) {
                var licenseInformation = currentApp.licenseInformation;
                !licenseInformation.isActive || licenseInformation.isTrial ? currentApp.requestAppPurchaseAsync(!1).done(function() {
                    licenseInformation.isActive && !licenseInformation.isTrial ? (WinJS.log && WinJS.log("You successfully upgraded your app to the fully-licensed version.", "sample", "status"), Game.purchaseReceived()) : WinJS.log && WinJS.log("You still have a trial license for this app.", "sample", "error")
                }, function() {
                    WinJS.log && WinJS.log("The upgrade transaction failed. You still have a trial license for this app.", "sample", "error")
                }) : (WinJS.log && WinJS.log("You already bought this app and have a fully-licensed version.", "sample", "error"), Game.purchaseReceived())
            }
        }, this.restorePurchases = restorePurchases, this.__defineGetter__("enabled", function() {
            return enabled
        })
    },
    BackgroundService = new function() {
        function createWorker() {
            if (supportsRuntimeWorker) {
                Game.debug && console.log("Web worker created on the fly");
                var js = [Utility, Grid, Tile, generateGridAndSolution, "var HintType = " + JSON.stringify(HintType) + ";var TileType = {Unknown: 'Unknown',Dot: 'Dot',Wall: 'Wall',Value: 'Value'};var Directions = {Left: 'Left',Right: 'Right',Up: 'Up',Down: 'Down'};", "\nvar Utils = new Utility();", "\nfunction Hint() { this.active = false; }", "self.onmessage = function(e) {generateGridAndSolution(e.data.size)};"].join(""),
                    blob = new Blob([js], {
                        type: "text/javascript"
                    });
                worker = new Worker(url.createObjectURL(blob))
            } else worker = new Worker("js/webworker.js"), Game.debug && console.log("Web worker created statically");
            worker.onmessage = function(e) {
                onPuzzleGenerated(JSON.parse(e.data))
            }
        }

        function onPuzzleGenerated(puzzle) {
            Game.debug && console.log("generated puzzle", puzzle), Levels.addSize(puzzle.size, puzzle)
        }

        function generatePuzzle(size) {
            enabled && (worker || createWorker(), worker.postMessage({
                size: size
            }))
        }
        var enabled = !!window.Worker,
            url = window.URL || window.webkitURL,
            supportsRuntimeWorker = !(!window.Blob || !url),
            worker = null;
        Game.debug && console.log("BackgroundService:", enabled), this.generatePuzzle = generatePuzzle, this.kick = function() {
            var needsSize = Levels.needs();
            if (needsSize) {
                if (window.FixedLevels && FixedLevels.has(needsSize)) {
                    var puzzle = FixedLevels.get(needsSize);
                    if (puzzle) return void Levels.addSize(puzzle.size, puzzle)
                }
                generatePuzzle(needsSize)
            }
        }, this.__defineGetter__("enabled", function() {
            return enabled
        })
    },
    Storage = new function() {
        function upgradeUserDataWithTemplateValues(templateData) {
            var changed = !1;
            for (var name in templateData) void 0 === data[name] && (data[name] = templateData[name], changed = !0, Game.debug && console.log("upgraded from template", name, templateData[name]));
            changed && save()
        }

        function save() {
            setItem(id, JSON.stringify(data))
        }

        function getItem(name, cb) {
            if ($.browser.chromeWebStore) chrome.storage.local.get(name, cb);
            else {
                var result = {};
                result[name] = localStorage.getItem(name), cb(result)
            }
        }

        function setItem(name, value, cb) {
            if ($.browser.chromeWebStore) {
                var command = {};
                command[name] = value, chrome.storage.local.set(command, cb)
            } else localStorage.setItem(name, value), cb && cb()
        }
        var id = "0hn0_storage",
            data = {
                q: 42,
                size4played: 0,
                size5played: 0,
                size6played: 0,
                size7played: 0,
                size8played: 0,
                size9played: 0,
                gamesPlayed: 0,
                bestTimeSize4: 240,
                bestTimeSize5: 300,
                bestTimeSize6: 360,
                bestTimeSize7: 420,
                bestTimeSize8: 480,
                bestTimeSize9: 540,
                gamesQuitted: 0,
                autoSignIn: !0,
                showTimeTrial: !1,
                achievementsUnlocked: {},
                achievementsNotSent: {},
                theme: 1,
                splashSkibbable: !1,
                donated: !1
            };
        $(function() {
            var templateData = JSON.parse(JSON.stringify(data));
            getItem(id, function(obj) {
                if (obj && obj[id]) {
                    var str = obj[id],
                        tempData = JSON.parse(str);
                    tempData && 42 == tempData.q && (data = tempData, upgradeUserDataWithTemplateValues(templateData))
                }
            })
        }), this.getItem = getItem, this.setItem = setItem, this.clear = function(cb) {
            $.browser.chromeWebStore ? chrome.storage.local.clear(cb) : (localStorage.clear(), cb && cb())
        }, this.levelCompleted = function(size, score, seconds, isTutorial, hintsUsed, undosUsed) {
            if (Game.debug && console.log("levelCompleted", size, score, seconds, isTutorial, hintsUsed, undosUsed), Game.debug && console.log("data", data), !(!size || size < 4 || size > 9) && seconds && !isNaN(seconds))
                if (isTutorial) PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.apprentice);
                else if (!(size < 4)) {
                if (data.gamesPlayed++, score > 0 && PlayCenter.submitScore(PlayCenter.IDS.Leaderboards.score, score), PlayCenter.submitScore(PlayCenter.IDS.Leaderboards.games_played, data.gamesPlayed), 4 == size && (data.size4played++, PlayCenter.submitScore(PlayCenter.IDS.Leaderboards._4_x_4_played, data.size4played), hintsUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.no_questions_asked_4_x_4), undosUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.forward_4_x_4)), 5 == size && (data.size5played++, PlayCenter.submitScore(PlayCenter.IDS.Leaderboards._5_x_5_played, data.size5played), hintsUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.no_questions_asked_5_x_5), undosUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.forward_5_x_5)), 6 == size && (data.size6played++, PlayCenter.submitScore(PlayCenter.IDS.Leaderboards._6_x_6_played, data.size6played), hintsUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.no_questions_asked_6_x_6), undosUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.forward_6_x_6)), 7 == size && (data.size7played++, PlayCenter.submitScore(PlayCenter.IDS.Leaderboards._7_x_7_played, data.size7played), hintsUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.no_questions_asked_7_x_7), undosUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.forward_7_x_7)), 8 == size && (data.size8played++, PlayCenter.submitScore(PlayCenter.IDS.Leaderboards._8_x_8_played, data.size8played), hintsUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.no_questions_asked_8_x_8), undosUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.forward_8_x_8)), 9 == size && (data.size9played++, PlayCenter.submitScore(PlayCenter.IDS.Leaderboards._9_x_9_played, data.size9played), hintsUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.no_questions_asked_9_x_9), undosUsed || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.forward_9_x_9)), 10 == data.gamesPlayed && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.ten), 42 == data.gamesPlayed && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.q42), 100 == data.gamesPlayed && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.hundred), !(data.gamesPlayed >= 1e3) || Storage.data.achievementsUnlocked && Storage.data.achievementsUnlocked.thousand || PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.thousand), 1 == data["size" + size + "played"]) {
                    for (var allFourUnlocked = !0, i = 5; i <= 8; i++) i != size && (data["size" + i + "played"] >= 1 || (allFourUnlocked = !1));
                    allFourUnlocked && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.allfour)
                }
                data["size" + size + "played"] >= 10 && (4 == size && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements._160_dots), 5 == size && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements._250_dots), 6 == size && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements._360_dots), 7 == size && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements._490_dots), 8 == size && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements._640_dots), 9 == size && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements._810_dots));
                var milliseconds = 1e3 * seconds;
                if (seconds > 0) {
                    PlayCenter.submitScore(PlayCenter.IDS.Leaderboards.best_time, milliseconds), 4 == size && PlayCenter.submitScore(PlayCenter.IDS.Leaderboards.best_time_4_x_4, milliseconds), 5 == size && PlayCenter.submitScore(PlayCenter.IDS.Leaderboards.best_time_5_x_5, milliseconds), 6 == size && PlayCenter.submitScore(PlayCenter.IDS.Leaderboards.best_time_6_x_6, milliseconds), 7 == size && PlayCenter.submitScore(PlayCenter.IDS.Leaderboards.best_time_7_x_7, milliseconds), 8 == size && PlayCenter.submitScore(PlayCenter.IDS.Leaderboards.best_time_8_x_8, milliseconds), 9 == size && PlayCenter.submitScore(PlayCenter.IDS.Leaderboards.best_time_9_x_9, milliseconds);
                    var currentBestTime = data["bestTimeSize" + size];
                    !isNaN(currentBestTime) && seconds < currentBestTime && seconds >= 1 && (data["bestTimeSize" + size] = seconds)
                }
                save()
            }
        }, this.gameQuitted = function() {
            data.gamesQuitted++, save(), 10 == data.gamesQuitted && PlayCenter.unlockAchievement(PlayCenter.IDS.Achievements.quitter)
        }, this.setDataValue = function(name, value) {
            data[name] = value, save()
        }, this.getDataValue = function(name, defaultValue) {
            return void 0 === data[name] ? void 0 != defaultValue && defaultValue : data[name]
        }, this.achievementUnlocked = function(id) {
            data.achievementsUnlocked[id] || (Game.debug && console.log("achievement unlocked: " + id), data.achievementsUnlocked[id] = !0, data.achievementsNotSent[id] = !0, save())
        }, this.achievementSent = function(id) {
            data.achievementsUnlocked[id] && data.achievementsNotSent[id] && (delete data.achievementsNotSent[id], save())
        }, this.__defineGetter__("data", function() {
            return data
        })
    },
    PlayCenter = new function() {
        this.IDS = {
            Leaderboards: {},
            Achievements: {}
        }, this.autoSignIn = function(handler) {}, this.signIn = function(auto, handler) {}, this.signOut = function(handler) {}, this.submitScore = function(leaderboardObj, score) {}, this.showLeaderboard = function(id) {}, this.unlockAchievement = function(achievementObj) {}, this.showAchievements = function() {}, this.resetAchievements = function() {}, this.__defineGetter__("enabled", function() {
            return !1
        }), this.__defineGetter__("isSignedIn", function() {
            return !1
        })
    },
    Themes = new function() {
        function cycle() {
            cycling && (cycling = !0, frameId = requestAnimFrame(cycle), cycleTime = new Date, grain())
        }

        function grain() {
            if (!(cycleTime < nextGrain)) {
                var x = Utils.between(-grainOffset, grainOffset, 4),
                    y = Utils.between(-grainOffset, grainOffset, 4),
                    opacity = Utils.between(29, 31) / 100;
                elGame.style.webkitTransform = "translate3d(" + x + "px, " + y + "px, 0)", elGame.style.transform = "translate3d(" + x + "px, " + y + "px, 0)", elGrain.style.opacity = opacity, nextGrain = new Date((new Date).getTime() + 50);
                for (i = 0; i < scratches.length; i++) {
                    var scratch = scratches[i];
                    ctx.clearRect(scratch[0], scratch[1], scratch[2] - scratch[0], scratch[3] - scratch[1])
                }
                scratches = [];
                var dotCount = Utils.between(1, 5);
                ctx.fillStyle = "rgba(0,0,0,0.3)";
                for (i = 0; i < dotCount; i++) {
                    x = Utils.between(0, viewport.width);
                    y = Utils.between(0, viewport.height), ctx.beginPath(), ctx.arc(x, y, Utils.between(.1, 3, 2), 0, pi2, !0), ctx.fill(), scratches.push([x - 4, y - 4, x + 4, y + 4])
                }
                var scratchCount = Utils.between(1, 5);
                ctx.lineWidth = .2, ctx.strokeStyle = "rgba(0,0,0,0.7)";
                for (var i = 0; i < scratchCount; i++) {
                    x = Utils.between(0, viewport.width);
                    y = Utils.between(0, viewport.height), dy = Utils.between(-viewport.height / 2, viewport.height, 2), ctx.beginPath(), ctx.moveTo(x, y), ctx.lineTo(x, y + dy), ctx.stroke();
                    var top = dy >= 0 ? y : y + dy,
                        bottom = dy >= 0 ? y + dy : y;
                    scratches.push([x - 1, top - 1, x + 1, bottom + 1])
                }
            }
        }
        var elGame, elGrain, canvas, pi2 = 2 * Math.PI,
            nextGrain = 0,
            ctx = null,
            scratches = [],
            grainOffset = 0,
            viewport = {
                width: 0,
                height: 0
            },
            cycling = !1,
            frameId = 0,
            hashEnabled = !1,
            hashMap = {
                1: "0hn0",
                2: "0hh1",
                3: "contranoid"
            };
        this.init = function() {
            $("#container").append('<canvas id="scratch-canvas"/>'), canvas = $("#scratch-canvas")[0], ctx = canvas.getContext("2d"), elGame = $("#gameContainer")[0], elGrain = $("#grain")[0];
            var theme = Storage.getDataValue("theme", 1);
            for (var id in hashMap) document.location.hash == "#" + hashMap[id] && (theme = id, hashEnabled = !0);
            Storage.setDataValue("theme", theme)
        }, this.cycle = cycle, this.grain = grain, this.resize = function(w, h) {
            viewport.width = w, viewport.height = h, canvas.width = w, canvas.height = h, grainOffset = .5 / 320 * w
        }, this.apply = function(theme) {
            3 == theme ? (cycling = !0, cycle()) : (cycling = !1, cancelAnimFrame && cancelAnimFrame(frameId)), hashEnabled && hashMap[theme] && (document.location.hash = hashMap[theme])
        }
    },
    Links = new function() {
        $(function() {
            var android = /android/.test(navigator.userAgent.toLowerCase()),
                ios = /ipad|iphone|ipod/.test(navigator.userAgent.toLowerCase());
            for (var id in links) {
                var $el = $("#game_link_" + id),
                    href = links[id].web;
                ios && (href = links[id].ios), android && (href = links[id].android), $el.attr("href", href)
            }
        });
        var links = {
            quento: {
                ios: "https://itunes.apple.com/us/app/quento/id583954698?mt=8",
                android: "https://play.google.com/store/apps/details?id=nl.q42.quento&hl=en",
                web: "http://quento.com"
            },
            numolition: {
                ios: "https://itunes.apple.com/us/app/numolition/id824164747?mt=8",
                android: "https://play.google.com/store/apps/details?id=com.q42.numolition",
                web: "http://numolition.com"
            },
            "0hn0": {
                ios: "https://itunes.apple.com/us/app/0h-n0/id957191082?mt=8",
                android: "https://play.google.com/store/apps/details?id=com.q42.ohno",
                web: "http://0hn0.com"
            },
            contranoid: {
                ios: "https://itunes.apple.com/us/app/contranoid/id1027717534?mt=8",
                android: "https://play.google.com/store/apps/details?id=com.q42.contranoid",
                web: "http://contranoid.com"
            },
            "0hh1": {
                ios: "https://itunes.apple.com/us/app/0h-h1/id936504196?mt=8",
                android: "https://play.google.com/store/apps/details?id=com.q42.ohhi",
                web: "http://0hh1.com"
            },
            flippybit: {
                ios: "https://itunes.apple.com/us/app/flippy-bit-attack-hexadecimals/id853100169?mt=8",
                android: "https://play.google.com/store/apps/details?id=com.q42.flippybitandtheattackofthehexadecimalsfrombase16&hl=en",
                web: "http://flippybitandtheattackofthehexadecimalsfrombase16.com"
            }
        }
    },
    Language = new function() {
        function detectLanguage() {
            $.getJSON("https://geoip.nekudo.com/api/", function(data) {
                if (Game.debug && console.log("succes", data), data && data.country && data.country.code) {
                    var lang = data.country.code.toLowerCase();
                    supported[lang] || (lang = defaultLang), Storage.setDataValue("language", lang), current = lang, refresh()
                }
            })
        }

        function refresh() {
            var toggleValue = current.toUpperCase();
            $("#toggleLanguage").html(toggleValue);
            var set = definitions[current];
            set && ($("text").each(function(i, t) {
                var $t = $(t),
                    id = $t.attr("data-text-id");
                id || (id = $t.html(), $t.attr("data-text-id", id)), translation = "en" == current ? id : set[id], $t.html(translation)
            }), Game.refreshGameAfterLanguageChange())
        }
        var supported = {
                en: 1,
                nl: 1
            },
            defaultLang = window.Config && Config.defaultLanguage ? Config.defaultLanguage : "en",
            current = defaultLang,
            definitions = {};
        $(function() {
            var userPrefLang = Storage.getDataValue("language");
            userPrefLang && supported[userPrefLang] ? (current = userPrefLang, refresh()) : (detectLanguage(), setTimeout(refresh, 100))
        }), this.add = function(lang, obj) {
            definitions[lang] = obj
        }, this.refresh = refresh, this.get = function(id) {
            var set = definitions[current];
            return set || (set = definitions[defaultLang]), set ? set[id] || id : id
        }, this.toggle = function() {
            current = "nl" == current ? "en" : "nl", Storage.setDataValue("language", current), refresh()
        }, this.__defineGetter__("current", function() {
            return current
        })
    };
Language.add("en", {
        weekDays: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
        ojoos: ["Wonderful", "Spectacular", "Marvelous", "Outstanding", "Remarkable", "Shazam", "Impressive", "Great", "Well done", "Fabulous", "Clever", "Dazzling", "Fantastic", "Excellent", "Nice", "Super", "Awesome", "Ojoo", "Brilliant", "Splendid", "Exceptional", "Magnificent", "Yay"],
        shareMsg: "#0hn0 It's 0h h1's companion! Go get addicted to this lovely puzzle game http://0hn0.com (or get the app)!",
        signInFailedIOS: "<p>Signing in with Game Center didn't work.</p><p>Please check your internet connection.</p><p>If this problem persists, open the Settings app, go to Game Center and sign in there.</p><p>If that doesn't work, give up and go play 0h h1 :)</p>",
        signInFailedAndroid: "<p>Signing in with Google Play Game Services didn't work.</p><p>Please check your internet connection.</p><p>If this problem persists, try again later or give up and go play 0h h1 :)</p>",
        OneDirectionLeft: 'Only one direction remains for this number to look in <span id="nextdot"></span>',
        ValueReached: 'This number can see all its dots <span id="nextdot" class="red"></span>',
        WouldExceed: 'Looking further in one direction would exceed this number <span id="nextdot" class="red"></span>',
        OneDirectionRequired: 'One specific dot is included <br>in all solutions imaginable <span id="nextdot"></span>',
        MustBeWall: 'This one should be easy... <span id="nextdot" class="red"></span>',
        ErrorClosedTooEarly: 'This number can\'t see enough <span id="nextdot"></span>',
        ErrorClosedTooLate: 'This number sees a bit too much <span id="nextdot" class="red"></span>',
        Error: 'This one doesn\'t seem right <span id="nextdot" class="red"></span>',
        Errors: 'These don\'t seem right <span id="nextdot" class="red"></span>',
        LockedIn: 'A blue dot should always see at least one other <span id="nextdot"></span>',
        GameContinued: 'You can now continue<br>your previous game <span id="nextdot"></span>',
        TimeTrialShown: 'Elapsed time is now shown. <br>Time to beat: %s <span id="nextdot"></span>',
        t01: 'Blue dots can see others <br>in their own row and column <span id="nextdot"></span>',
        t02: 'Red dots block their view <span id="nextdot" class="red"></span>',
        t03: 'Numbers tell how many others each blue dot can see <span id="nextdot"></span>',
        t05: 'This 2 says it sees two dots <br>so they must be on the right <span id="nextdot"></span>',
        t06: 'These two. <br>Tap to make them blue <span id="nextdot"></span>',
        t07: 'Now close its path.<br>Tap twice for a red dot <span id="nextdot" class="red"></span>',
        t08: 'This 1 can see only one. <br>It already does - below <span id="nextdot"></span>',
        t09: 'So a red dot must be <br>blocking its view here <span id="nextdot" class="red"></span>',
        t10: 'This 3 can\'t see left or right.<br>But it does see a dot above <span id="nextdot"></span>',
        t11: 'To make it see three dots <br>it needs two more... <span id="nextdot"></span>',
        t12: 'Can you fill out <br>the remaining dots? <span id="nextdot" class="red"></span>',
        "That's the undo button.": "That's the undo button.",
        "Nothing to undo.": "Nothing to undo.",
        "This tile was reversed to ": "This tile was reversed to ",
        "blue.": "blue.",
        "red.": "red.",
        "its empty state.": "its empty state."
    }), Language.add("nl", {
        weekDays: ["Zondag", "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrijdag", "Zaterdag"],
        ojoos: ["Goed", "Netjes", "Knap", "Fantastisch", "Geweldig", "Super", "Slim", "Briljant"],
        shareMsg: "#0hn0 shareMsg!",
        signInFailedIOS: "Inloggen niet gelukt!",
        signInFailedAndroid: "Inloggen niet gelukt!",
        OneDirectionLeft: 'Deze stip kan maar in één richting een volgende stip zien <span id="nextdot"></span>',
        ValueReached: 'Deze stip ziet alle stippen al <span id="nextdot" class="red"></span>',
        WouldExceed: 'Als deze stip verder kijkt in een richting dan ziet hij te veel <span id="nextdot" class="red"></span>',
        OneDirectionRequired: 'Alle oplossingen die je bedenkt hebben één stip sowieso nodig <span id="nextdot"></span>',
        MustBeWall: 'Deze moet makkelijk zijn... <span id="nextdot" class="red"></span>',
        ErrorClosedTooEarly: 'Deze stip ziet nog niet alles <span id="nextdot"></span>',
        ErrorClosedTooLate: 'Deze stip ziet iets te veel <span id="nextdot" class="red"></span>',
        Error: 'Hiet lijkt iets niet te kloppen <span id="nextdot" class="red"></span>',
        Errors: 'Deze lijken niet te kloppen <span id="nextdot" class="red"></span>',
        LockedIn: 'Een blauwe stip kan altijd wel één andere stip zien <span id="nextdot"></span>',
        GameContinued: 'Je kunt nu verder gaan<br>met je vorige spel <span id="nextdot"></span>',
        TimeTrialShown: 'Verstreken tijd wordt getoond. <br>Beste tijd: %s <span id="nextdot"></span>',
        t01: 'Een blauwe stip kan anderen zien in dezelfde rij en kolom <span id="nextdot"></span>',
        t02: 'Rode stippen blokkeren het zicht als een soort muurtjes <span id="nextdot" class="red"></span>',
        t03: 'Getallen zeggen hoeveel andere blauwe stippen een stip ziet <span id="nextdot"></span>',
        t05: 'Deze 2 ziet dus twee anderen. <br>Die moeten rechts ervan staan <span id="nextdot"></span>',
        t06: 'Het zijn deze twee stippen. <br>Tap om ze blauw te maken <span id="nextdot"></span>',
        t07: 'Deze stip moet dus rood zijn.<br>Tap twee keer voor rood <span id="nextdot" class="red"></span>',
        t08: 'Deze 1 ziet er maar één. <br>En die staat er al onder! <span id="nextdot"></span>',
        t09: 'Dus moet hier een rode stip<br>het zicht blokkeren <span id="nextdot" class="red"></span>',
        t10: 'Deze 3 ziet niets links of rechts.<br>Maar hij ziet er 1 boven zich <span id="nextdot"></span>',
        t11: 'Om hem drie te laten zien <br>heeft hij er nog twee nodig... <span id="nextdot"></span>',
        t12: 'Kun jij de rest invullen? <span id="nextdot" class="red"></span>',
        "Select a size": "Kies formaat",
        "It's 0h h1's companion!": "Het vervolg op 0h h1",
        "A game by Q42": "Een spel van Q42",
        "Created by Martin Kool": "Gemaakt door Martin Kool",
        "By Q42": "Door Q42",
        "Made by Q42.": "Gemaakt door Q42.",
        New: "Nieuw",
        "How to play": "Uitleg",
        "Free play": "Vrij spelen",
        Play: "Spelen",
        Settings: "Instellingen",
        About: "Over",
        Thanks: "Bedankt",
        "0h n0 is a little logic game<br>that complements 0h h1.<br>It was created by<br>Martin Kool.": "0h n0 is een klein logica spel,<br>net als voorganger 0h h1.<br>Het is gemaakt door<br>Martin Kool.",
        "The concept is inspired by <br>the Japanese Kuromasu.": "Het concept vindt zijn oorsprong in het Japanse Kuromasu.",
        "by Q42": "door Q42",
        Rules: "Spelregels",
        "Blue dots can see others <br>in their own row and column": "Blauwe stippen zien anderen <br>in hun eigen rij en kolom",
        "They see at least one": "Ze zien er minimaal één",
        "Their numbers tell how many": "Hun getal zegt hoeveel ze zien",
        "Red dots block their view": "Rode stippen blokkeren hun zicht",
        "Thank you for supporting<br> 0h n0": "Bedankt dat je<br> 0h n0 support",
        "Please feel free to leave me a message about the game anytime": "Laat gerust een keer wat van je horen",
        Apps: "Apps",
        "The 0h n0 app is also free": "Er is een gratis 0h n0 app",
        "The source is on github": "De source staat op Github",
        "Our game 0h h1 is also available as a free app": "Ons spel 0h h1 is ook beschikbaar als gratis app",
        "And playable on the web": "En speelbaar op het web",
        Games: "Spellen",
        "Enjoy our other games too": "Speel ook onze andere spellen",
        "This is a message": "Dit is een bericht",
        Loading: "Momentje",
        "Select a size to play...": "Welk formaat wil je spelen?",
        "if 0h n0 is worth a $": "als 0h n0 een € waard is",
        "tap here": "tap hier",
        "You are currently signed in": "Je bent ingelogd",
        "Sign out": "Uitloggen",
        "Sign in to unlock fun achievements, earn experience points and compete with friends in leaderboards": "Log in om leuke achievements te behalen, XP te verdienen en te strijden tegen je vrienden in de ranglijsten",
        "Sign in": "Inloggen",
        Settings: "Instellingen",
        "Show elapsed time": "Toon verstreken tijd",
        "Show hint icon": "Toon hint icoon",
        "Color theme": "Kleurthema",
        "0h n0 is worth a $": "0h n0 is een € waard",
        No: "Nee",
        Yes: "Ja",
        Language: "Taal",
        "Q42.com": "Q42.nl",
        "That's the undo button.": "Dat is de undo knop.",
        "Nothing to undo.": "Geen undo mogelijk.",
        "This tile was reversed to ": "Deze stip is teruggezet naar ",
        "blue.": "blauw.",
        "red.": "rood.",
        "its empty state.": "een leeg vakje.",
        "Today's puzzles": "Puzzels van vandaag"
    }),
    function(i, s, o, g, r, a, m) {
        i.GoogleAnalyticsObject = r, i[r] = i[r] || function() {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date, a = s.createElement(o), m = s.getElementsByTagName(o)[0], a.async = 1, a.src = "js/analytics.js", m.parentNode.insertBefore(a, m)
    }(window, document, "script", 0, "ga");
var Analytics = {
        init: function() {
            var uuid = Storage.getDataValue("uuid", null);
            uuid || (uuid = generateUUID(), Storage.setDataValue("uuid", uuid));
            var ua = Config.GoogleAnalytics;
            ga("create", ua, {
                storage: "none",
                clientId: uuid
            }), ga("set", "checkProtocolTask", null), ga("set", "checkStorageTask", null), ga("set", "anonymizeIp", !0), ga("send", "pageview", {
                page: "/"
            })
        }
    },
    Marker = new function() {
        this.save = function(category, action, label, value) {
            ga && (Game.debug && console.log(category, action, label, value), ga && ga("send", "event", category, action, label, value))
        }
    };
$(app.onDeviceReady);