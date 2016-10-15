如果数据库中为 ‘i', 'q', 'z' statusDb

数据实体中：
enum status {
  index = 'i',
  querty = 'q',
  zwrer = 'z'
}

在查询sql数据时可以这样处理：ASCII(statusDb),就能正常的处理
