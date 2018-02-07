select 
	new.SysNo,
	new.NewsCategorySysNo,
	new.Title,
	new.Subtitle,
	new.ExpireTime,
	new.TopMost,
	new.CoverType,
	new.CoverSerialize,
	new.Content,
	new.Source,
	new.Status,
	new.PublishTime,
	new.LastPublishTime,	
	new.ReadQty,
	new.Priority as Priority,
	category.name as CategoryName
from 
(select a.* 
    from ContentManagement.News a 
    where (select count(1) from ContentManagement.News 
    where NewsCategorySysNo = a.NewsCategorySysNo and Priority >= a.Priority and  LastPublishTime >= a.LastPublishTime and status=2  ) <= 3
    and a.status=2
)new
inner join ContentManagement.NewsCategory category on category.SysNo=new.NewsCategorySysNo
where category.status = 1
order by new.NewsCategorySysNo,new.Priority desc, new.LastPublishTime DESC


/*
按NewsCategorySysNo分组，以Priority为优先级, LastPublishTime先后顺序，取三条数据
*/
