Vue.filter('PriceFixedFilter', (value, point) => {
  var tempValue = 0
  if (typeof value === 'string') {
    tempValue = Number(value);
  } else {
    tempValue = value;
  }
  var tempFixedValue = Math.abs(tempValue).toFixed(point);
  return "￥" + tempFixedValue.replace(/(\d{1,3})(?=(\d{3})+(?:$|\.))/g,'$1,');
})
