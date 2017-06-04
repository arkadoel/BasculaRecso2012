SET DATEFORMAT dmy;
select top 5
	HistoricoAlbaranes.obra,
	sum(cast(replace(importeFinal,',','.') as float)) as sumaImporte
	
from HistoricoAlbaranes
where convert(datetime, replace(fechaSalida,'/','-')) >= '@finicio' 
and convert(datetime, replace(fechaSalida,'/','-')) <= '@ffin'
AND [tipoResiduo]='ENTRADA' 
group by obra
order by 2 desc