(function() {
    function Renderer(options) {
        let defaults = {
            width: 800,
            height: 500,
            pixelSize: 1,
            matrix: [],
        }

        let el = document.getElementById(options.el);
        let canvas = document.createElement('canvas');
        canvas.width = options.width || defaults.width;
        canvas.height = options.height || defaults.height;
        el.appendChild(canvas);

        this.cnv = canvas;
        this.ctx = canvas.getContext('2d');
        this.pixelSize = options.pixelSize || defaults.pixelSize;
        this.matrix = options.matrix || defaults.matrix;

        this.render()
    }

    Renderer.prototype.render = function() {
        let arr = this.matrix;
        for (let i = 0; i < arr.length; i++) {
            for (let j = 0; j < arr[i].length; j++) {
                this.ctx.fillStyle = arr[i][j];
                this.ctx.fillRect(
                    i * this.pixelSize,
                    this.cnv.height - this.pixelSize - j * this.pixelSize,
                    this.pixelSize,
                    this.pixelSize
                );
            }
        }
    }

    Renderer.prototype.render2DArray = function(arr) {
        this.matrix = arr;
        this.render();
    }

    window.Renderer = Renderer;
})();