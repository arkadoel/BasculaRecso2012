SET DATEFORMAT dmy;
/*formato dia '2012-24-04'*/
select top 10
	residuo, 
	sum(cast(neto as int)) as toneladas, 
	cast(replace(precioResiduo,',','.') as float) as precio, 
	sum(CAST( replace(importeFinal,',','.') as float)) as sumaImporte 
from HistoricoAlbaranes
where convert(datetime, replace(fechaSalida,'/','-')) >= '@finicio' 
and convert(datetime, replace(fechaSalida,'/','-')) <= '@ffin'
group by residuo, precioResiduo
order by 2 desc

