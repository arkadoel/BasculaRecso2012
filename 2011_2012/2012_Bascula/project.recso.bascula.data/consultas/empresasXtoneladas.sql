SET DATEFORMAT dmy;
select top 5
	empPoseedor, 
	sum(cast(neto as int)) as sumaNeto
	
from HistoricoAlbaranes
where convert(datetime, replace(fechaSalida,'/','-')) >= '@finicio' 
and convert(datetime, replace(fechaSalida,'/','-')) <= '@ffin'
AND [tipoResiduo]='ENTRADA' 
group by empPoseedor
order by 2 desc