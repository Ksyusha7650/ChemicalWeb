﻿function IsGood(min, max, text) {
    let d;
    try {
        d = parseFloat(text.value);
    } catch {
        text.innerHTML = 0;
        // text.style.color = "red";
        return false;
    }
    if (min <= d && d <= max) {
        // text.style.color = "black";
        return true;
    }
    // text.style.color = "red";
    return false;
}

class Material {
    constructor(p = 0, c = 0, t0 = 0) {
        this.P = p;
        this.C = c;
        this.T0 = t0;
    }
}

class Channel {
    constructor(w = 0, h = 0, l = 0) {
        this.W = w;
        this.H = h;
        this.L = l;
    }
}

class Calc {
    R = 8.314;
    Step = 0.01;

    Material;
    Channel;

    Vu = 1.2;
    Tu = 220;
    Mu0 = 9000;
    Ea = 92000;
    Tr = 210;
    N = 0.3;
    AlphaU = 450;

    WMin = 0.0001;
    HMin = 0.0001;
    LMin = 0.01;
    StepMin = 0.001;
    PMin = 100;
    CMin = 100;
    T0Min = 30;
    VuMin = 0.01;
    TuMin = 30;
    Mu0Min = 100;
    EaMin = 100;
    TrMin = 30;
    NMin = 0.01;
    AlphaUMin = 10;

    WMax = 3;
    HMax = 3;
    LMax = 10;
    StepMax = 2;
    PMax = 20000;
    CMax = 10000;
    T0Max = 1000;
    VuMax = 10;
    TuMax = 1000;
    Mu0Max = 50000;
    EaMax = 1000000;
    TrMax = 1000;
    NMax = 1;
    AlphaUMax = 5000;

    constructor(channel = new Channel(), step = 0, material = new Material(), Vu = 0, Tu = 0, Mu0 = 0, Ea = 0, Tr = 0, N = 0, AlphaU = 0) {
        this.Step = step;
        this.Material = material;
        this.Channel = channel;
        this.Vu = Vu;
        this.Tu = Tu;
        this.Mu0 = Mu0;
        this.Ea = Ea;
        this.Tr = Tr;
        this.N = N;
        this.AlphaU = AlphaU;
    }

    MaterialShearStrainRate() {
        this._gammaPoint = this.Vu / this.Channel.H;
    }

    SpecificHeatFluxes() {
        this._qGamma =
            this.Channel.H *
            this.Channel.W *
            this.Mu0 *
            Math.pow(this._gammaPoint, this.N + 1);
        this._beta =
            this.Ea /
            (this.R * (this.Material.T0 + 20 + 273) * (this.Tr + 273));
        this._qAlpha =
            this.Channel.W *
            this.AlphaU *
            (Math.pow(this._beta, -1) - this.Tu + this.Tr);
    }

    VolumeFlowRateOfMaterialFlowInTheChannel() {
        this._f =
            0.125 * Math.pow(this.Channel.H / this.Channel.W, 2) -
            0.625 * (this.Channel.H / this.Channel.W) +
            1;
        this._qch = (this.Channel.H * this.Channel.W * this.Vu * this._f) / 2;
    }

    Temperature(z) {
        return (
            this.Tr +
            (1 / this._beta) *
            Math.log(
                ((this._beta * this._qGamma + this.Channel.W * this.AlphaU) /
                    (this._beta * this._qAlpha)) *
                (1 -
                    Math.exp(
                        (-this._beta *
                            this._qAlpha *
                            z) /
                        (this.Material.P * this.Material.C * this._qch)
                    )
                ) +
                Math.exp(
                    this._beta *
                    (this.Material.T0 -
                        this.Tr -
                        (this._qAlpha * z) /
                        (this.Material.P * this.Material.C * this._qch))
                )
            )
        );
    }

    Viscosity(T) {
        return (
            this.Mu0 *
            Math.exp(-this._beta * (T - this.Tr)) *
            Math.pow(this._gammaPoint, this.N - 1)
        );
    }

    Efficiency() {
        this.Q =
            Math.round(
                this.Material.P * this._qch,
                2
            ) * 3600;
        return this.Q;
    }
}


let result = document.querySelectorAll('.res div');

let _zCoord;
let _temperature;
let _viscosity;
let _q;
let _currentMaterial;
let _calc = new Calc();

function CalculateLists() {
    let W = document.querySelector('.W');
    let H = document.querySelector('.H');
    let L = document.querySelector('.L');
    let step = document.querySelector('.step');
    let P = document.querySelector('.P');
    let c = document.querySelector('.C');
    let T0 = document.querySelector('.T0');
    let Vu = document.querySelector('.Vu');
    let Tu = document.querySelector('.Tu');
    let mu0 = document.querySelector('.mu0');
    let Ea = document.querySelector('.Ea');
    let Tr = document.querySelector('.Tr');
    let n = document.querySelector('.n');
    let alphaU = document.querySelector('.alphaU');
    let MaterialComboBox = document.querySelector('.MaterialComboBox');
    let isGoodData = IsGood(_calc.WMin, _calc.WMax, W);
    if (!IsGood(_calc.HMin, _calc.HMax, H)) isGoodData = false;
    if (!IsGood(_calc.LMin, _calc.LMax, L)) isGoodData = false;
    if (!IsGood(_calc.StepMin, _calc.StepMax, step)) isGoodData = false;
    if (!IsGood(_calc.PMin, _calc.PMax, P)) isGoodData = false;
    if (!IsGood(_calc.CMin, _calc.CMax, c)) isGoodData = false;
    if (!IsGood(_calc.T0Min, _calc.T0Max, T0)) isGoodData = false;
    if (!IsGood(_calc.VuMin, _calc.VuMax, Vu)) isGoodData = false;
    if (!IsGood(_calc.TuMin, _calc.TuMax, Tu)) isGoodData = false;
    if (!IsGood(_calc.Mu0Min, _calc.Mu0Max, mu0)) isGoodData = false;
    if (!IsGood(_calc.EaMin, _calc.EaMax, Ea)) isGoodData = false;
    if (!IsGood(_calc.TrMin, _calc.TrMax, Tr)) isGoodData = false;
    if (!IsGood(_calc.NMin, _calc.NMax, n)) isGoodData = false;
    if (!IsGood(_calc.AlphaUMin, _calc.AlphaUMax, alphaU)) isGoodData = false;
    if (MaterialComboBox.value === "") {
        isGoodData = false;
    }
    if (!isGoodData) return false;
    let material = new Material(parseFloat(P.value), parseFloat(c.value), parseFloat(T0.value));
    let channel = new Channel(parseFloat(W.value), parseFloat(H.value), parseFloat(L.value));
    _calc = new Calc(channel, parseFloat(step.value), material, parseFloat(Vu.value), parseFloat(Tu.value), parseFloat(mu0.value), parseFloat(Ea.value), parseFloat(Tr.value), parseFloat(n.value), parseFloat(alphaU.value));
    _zCoord = [];
    _temperature = [];
    _viscosity = [];
    _q = [];
    _calc.MaterialShearStrainRate();
    _calc.SpecificHeatFluxes();
    _calc.VolumeFlowRateOfMaterialFlowInTheChannel();
    for (let z = 0; z <= _calc.Channel.L; z += _calc.Step) {
        _zCoord.push(z.toFixed(3));
        let temperature = _calc.Temperature(z);
        _temperature.push(temperature.toFixed(3));
        let viscosity = Math.round(_calc.Viscosity(temperature), 2);
        _viscosity.push(viscosity.toFixed(3));
        _q.push(_calc.Efficiency());
    }
    return true;
}

function TableWork() {
    let table = document.querySelector('.table');
    table.innerHTML = '';
    let thead = document.createElement('thead');
    let tbody = document.createElement('tbody');

    table.appendChild(thead);
    table.appendChild(tbody);

    let row_1 = document.createElement('tr');
    let heading_1 = document.createElement('th');
    heading_1.innerHTML = "Координата по длине канала, м";
    let heading_2 = document.createElement('th');
    heading_2.innerHTML = "Температура, °C";
    let heading_3 = document.createElement('th');
    heading_3.innerHTML = "Вязкость, Па*c";

    row_1.appendChild(heading_1);
    row_1.appendChild(heading_2);
    row_1.appendChild(heading_3);
    thead.appendChild(row_1);

    for (let i = 0; i < _temperature.length; i++) {
        let row = document.createElement('tr');
        let row_data_1 = document.createElement('td');
        row_data_1.innerHTML = _zCoord[i];
        let row_data_2 = document.createElement('td');
        row_data_2.innerHTML = _temperature[i];
        let row_data_3 = document.createElement('td');
        row_data_3.innerHTML = _viscosity[i];

        row.appendChild(row_data_1);
        row.appendChild(row_data_2);
        row.appendChild(row_data_3);
        tbody.appendChild(row);
    }
}

function CanvasWork(Xarr, Yarr, canvas, yMax) {
    // var canvas = document.querySelector(".one");
    var ctx = canvas.getContext("2d");

    var dataPoints = [];

    for(let i = 0; i < Xarr.length; i++){
        dataPoints.push({x: Xarr[i], y : Yarr[i]});
    }

    // var yMax = 300; // максимальное значение по y
    var yMin = 0; // минимальное значение по y

    // отступы по краям графика
    var padding = {
        top: 10,
        right: 10,
        bottom: 20,
        left: 40
    };

    // масштабирование точек по осям координат
    var scaleX = (canvas.width - padding.left - padding.right) / (dataPoints.length - 1);
    var scaleY = (canvas.height - padding.top - padding.bottom) / (yMax - yMin);

    // рисование осей координат
    ctx.beginPath();
    ctx.moveTo(padding.left, canvas.height - padding.bottom);
    ctx.lineTo(canvas.width - padding.right, canvas.height - padding.bottom);
    ctx.moveTo(padding.left, padding.top);
    ctx.lineTo(padding.left, canvas.height - padding.bottom);
    ctx.stroke();

    // рисование точек на графике
    ctx.beginPath();
    ctx.moveTo(getX(0), getY(dataPoints[0].y));
    for (var i = 1; i < dataPoints.length; i++) {
        ctx.lineTo(getX(i), getY(dataPoints[i].y));
    }
    ctx.stroke();

    // получение координат x и y для точки графика
    function getX(i) {
        return i * scaleX + padding.left;
    }

    function getY(value) {
        return canvas.height - ((value - yMin) * scaleY + padding.bottom);
    }

}

let buttonCulc = document.querySelector(".culc");
buttonCulc.addEventListener("click", function () {
    if (CalculateLists()) {
        TableWork();
        var canvas = document.querySelector(".one");
        CanvasWork(_zCoord, _temperature, canvas, 300);
        canvas = document.querySelector(".two");
        CanvasWork(_zCoord, _viscosity, canvas, 1200);
        result[0].innerHTML = `Производительность: ${_calc.Efficiency()} кг/ч`;
        result[1].innerHTML = `Температура: ${parseFloat(_temperature[_temperature.length - 1]).toFixed(2)} °С`;
        result[2].innerHTML = `Вязкость: ${parseFloat(_viscosity[_viscosity.length - 1]).toFixed(2)} Па*с`;
    }
});

let buttonExit = document.querySelector(".exit");
buttonExit.addEventListener("click", function () {
    window.location.href = "index.html";
});