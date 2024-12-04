const timeText = document.querySelector('#time-text')

setInterval(()=>{
    const date = new Date();
    timeText.textContent = `Thời gian: ${date.getDate()}/${date.getMonth()+1}/${date.getFullYear()}, ${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`
},1000)