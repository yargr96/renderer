(function() {
    function Renderer(options) {
        let el = document.getElementById(options.el);
        let canvas = document.createElement('canvas');
        canvas.width = options.width || 500;
        canvas.height = options.height || 500;
        el.appendChild(canvas);

        this.cnv = canvas;
        this.ctx = canvas.getContext('2d');
        this.pixelSize = options.pixelSize || 1;
    }

    Renderer.prototype.render2DArray = function(arr) {
        for (let i = 0; i < arr.length; i++) {
            for (let j = 0; j < arr[i].length; j++) {
                this.ctx.fillStyle = arr[i][j];
                this.ctx.fillRect(
                    i * this.pixelSize,
                    j * this.pixelSize,
                    this.pixelSize,
                    this.pixelSize
                );
            }
        }
    }

    window.Renderer = Renderer;
})();


let rdr = new Renderer({
    el: 'main',
    pixelSize: 20
})

rdr.render2DArray([
    [
        "#000",
        "#fff",
        "#000",
        "#002345",
    ],
    [
        "#000",
        "#fff",
        "#000",
    ],
    [
        "#000",
        "#000",
        "#000",
    ],
])
