function setTheme(name) {
    document.documentElement.setAttribute('data-theme', name);
    localStorage.setItem('theme', name);
    document.getElementById('currentTheme').textContent = name;

    document.querySelectorAll('.theme-btn').forEach(btn => {
        const isActive = btn.dataset.theme === name;
        btn.classList.toggle('active', isActive);
        btn.setAttribute('aria-pressed', isActive ? 'true' : 'false');
    });
}

function weatherCodeToText(code) {
    const map = {
        0: 'Ясно',
        1: 'Преимущественно ясно',
        2: 'Переменная облачность',
        3: 'Пасмурно',
        45: 'Туман',
        48: 'Изморозь',
        51: 'Лёгкая морось',
        53: 'Умеренная морось',
        55: 'Сильная морось',
        61: 'Лёгкий дождь',
        63: 'Умеренный дождь',
        65: 'Сильный дождь',
        71: 'Лёгкий снег',
        73: 'Умеренный снег',
        75: 'Сильный снег',
        80: 'Ливень',
        81: 'Сильный ливень',
        82: 'Очень сильный ливень',
        95: 'Гроза',
        96: 'Гроза с градом',
    };
    return map[code] ?? `Код ${code}`;
}

function incrementWeatherFetchCount() {
    let n = Number(sessionStorage.getItem('requestCount') ?? 0);
    sessionStorage.setItem('requestCount', ++n);
    document.getElementById('sessionCount').textContent = n;
}

async function loadState() {
    const renderHistory = (cities) => {
        document.getElementById('cityHistory').innerHTML = cities.length > 0 ? cities.map(c => `<li>${c}</li>`).join('') : '<li>—</li>';
        console.log(cities);
    }
    try {
        const state = await fetch('/api/weather/state').then(x => x.json());

        renderHistory(state.recentCitiesFromSession ?? []);
        document.getElementById('cookieCity').innerHTML = state?.currentCityFromCookie ?? '-';
    } catch {
        renderHistory([]);
        document.getElementById('cookieCity').textContent = '—';
    }
}

async function fetchWeather() {
    const city = document.getElementById('citySelect').value;

    const resultDiv = document.getElementById('weatherResult');
    resultDiv.className = 'card';
    resultDiv.innerHTML = 'Загрузка...';

    try {
        const data = await fetch(`/api/weather/current?city=${encodeURIComponent(city)}`).then(x => x.json());

        resultDiv.innerHTML = `
            <p class="temp">${data.temperature}°C</p>
            <p><strong>${data.city}</strong></p>
            <p class="meta">${weatherCodeToText(data.description)} &middot; Ветер ${data.windSpeed} км/ч</p>`;

        incrementWeatherFetchCount();
        await loadState();

    } catch {
        resultDiv.innerHTML = `<p style="color:red">Ошибка запроса</p>`;
    }
}

setTheme(localStorage.getItem('theme') || 'light');
fetchWeather();