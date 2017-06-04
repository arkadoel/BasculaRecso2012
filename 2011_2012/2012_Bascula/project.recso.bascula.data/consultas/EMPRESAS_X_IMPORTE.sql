SET DATEFORMAT dmy;
select top 5
	HistoricoAlbaranes.empPoseedor,
	sum(cast(replace(importeFinal,',','.') as float)) as sumaImporte
	
from HistoricoAlbaranes
where convert(datetime, replace(fechaSalida,'/','-')) >= '@finicio' 
and convert(datetime, replace(fechaSalida,'/','-')) <= '@ffin'
AND [tipoResiduo]='ENTRADA' 
group by empPoseedor
order by 2 desc