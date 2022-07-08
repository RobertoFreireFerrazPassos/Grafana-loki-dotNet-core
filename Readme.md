# Project Overview

This projects uses **Docker** (with volumes to keep the data logs and grafana settings), **Loki** (to save log), **Grafana** (to visualize the log), **WebApi in AspNet Core using Serilog** (log library).

## To do:


#### Dashboard

- Add how to create dashboard with painels using queries and metrics

#### Alert

- Create a alert

_____________________________________________


# Steps to Run:

1 - Using Visual Studio:
```
Set docker-compose as start up project on Visual Studio 
Click on Docker compose
```

2 - Using Cmd
```
Open cmd at folder ...\Grafana-loki-dotNet-core\ApplicationWeb
Run command "docker compose up"
```

## Grafana: 

#### Add new Data Source:
```
Link: http://localhost:3000/
Default user name and password will be "admin"

click on "Add new Data Source"
Name: ApplicationWeb
Url: http://Loki:3100

click on "Save & test"
```

#### Run query
```
After run some endpoint in swagger:
Go to Explore, 
In Log Browser, Run query 
{Application="ApplicationWeb"}
```

## Swagger:

Run some endpoints to see new logs in the Grafana
```
ApplicationWeb (ApiTest)
http://localhost:4001/swagger/index.html
```

#### Endpoints:

1 - ApiTest/SaveData
```
It logs the data request
```
2 - ApiTest/GetResult
```
It logs some new data and returns it
```
3 - ApiTest/BadResult
```
It logs information about error and returns bad request
```
4 - ApiTest/Exception
```
It throws error, logs error and returns bad request
```
5 - ApiTest/GetValue
```
Generates number between 0 and 100, logs this number and returns it
```


_____________________________________________


# LogQL

The basic idea of querying logs in loki in Grafana is to query, filter, parse, format and agregate the logs

LogQL (Grafana Loki's query language) uses **labels** to facilitate filtering log lines

There are two types of LogQL queries:

**Log queries** return the contents of log lines.

**Metric queries** extend log queries to calculate values based on query results.

## Log Queries

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/log_queries.PNG?raw=true">
</p>


### Stream Selector

**Label matching operators**
```
=: exactly equal
!=: not equal
=~: regex matches
!~: regex does not match
```

Ex: Query by Application and StatusCode
```
{Application="ApplicationWeb",StatusCode="200"}
```

Ex: Query by Application and both 200 and 400 status codes
```
{Application="ApplicationWeb",StatusCode=~"200|400"}
```

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/regexexample1.PNG?raw=true">
</p>

### Log pipeline: Line filter expressions

It filters the log line as a string

```
|=: Log line contains string
!=: Log line does not contain string
|~: Log line contains a match to the regular expression
!~: Log line does not contain a match to the regular expression
```

Ex: {Application="ApplicationWeb"} |= "Conten"

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/linefilterexpressionexample1.PNG?raw=true">
</p>

Ex: {Application="ApplicationWeb"} |~ "Request .* HTTP"

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/linefilterexpressionexample2.PNG?raw=true">
</p>

### Log pipeline: Parsing expressions

#### | json

Note: the parsing expression **doesn't** create a JSON object from a string log line. it obtains the log lines labels for further processing.

It is **not** possible to filter by "Detected fields"

Wrong example: {Application="ApplicationWeb", LogId="78b952af-bd57-45da-8c8c-52171c952b73"} 

First, it must parse using '| json', so "Detected fields" become "Log labels". Then, it can filter.
```
{Application="ApplicationWeb"} | json | LogId="292d74e6-3899-439d-876f-f99596d350a0"
```

Also, for a string object, the expression **| json** will produce the following mapping: { "a.b": {c: "d"}, e: "f" } -> {a_b_c="d", e="f"}

Ex: {Application="ApplicationWeb"} | json
```
Content { "Result" : 34 } -> Content_Result 34
```
<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/jsonexample2.PNG?raw=true">
</p>

It can create new labels using **| json the_new_label = "the_existing_label"**

Ex: {Application="ApplicationWeb"} | json | LogId="292d74e6-3899-439d-876f-f99596d350a0" | json new_label="Content"

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/jsonexample1.PNG?raw=true">
</p>

It only supports field access (my.field, my["field"]) and array access (list[0]), and any combination of these in any level of nesting (my.list[0]["field"])

Ex: {Application="ApplicationWeb"} | json | LogId="da56018a-cdfb-4c57-8c0d-029c9ef9970d" | json new_label="Content", label_result="Content.Result"

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/jsonexample3.PNG?raw=true">
</p>

Note: There are also another parsers:
```
| logfmt : extract all keys and values from the logfmt formatted log line.
| pattern "<pattern-expression>" : The expression matches the structure of a log line.
| regexp "<re>" which is the regular expression using the Golang RE2 syntax.
| unpack : parses a JSON log line, unpacking all embedded labels in the pack stage
```


### Log pipeline: Label filter expressions

It filter the log lines by filtering the corresponding labels.

1 - For labels with **string** values, uses the **label matching operators** (=, !=, =~ or !~)

Ex: {Application="ApplicationWeb"} | json | SourceContext != "Microsoft.AspNetCore.Hosting.Diagnostics"

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/labelfilteroperationexample2.PNG?raw=true">
</p>

Note: The above query could be replaced by {Application="ApplicationWeb", SourceContext != "Microsoft.AspNetCore.Hosting.Diagnostics"} since some of the log lines have this 'SourceContext' label

2 - For labels with **duration**, **number** or **bytes** values.

```
== or = for equality.
!= for inequality.
> and >= for greater than and greater than or equal.
< and <= for lesser than and lesser than or equal.
```

Ex: {Application ="ApplicationWeb"} | StatusCode = 400

Same result as: {Application ="ApplicationWeb", StatusCode="400"} 

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/labelfilteroperationexample1.PNG?raw=true">
</p>

Ex: {Application="ApplicationWeb"} | json | SourceContext != "Microsoft.AspNetCore.Hosting.Diagnostics" | Content_Result > 34

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/labelfilteroperationexample3.PNG?raw=true">
</p>

### Log pipeline: Line format expressions 

It can mutate the log content using **line_format "{{.Label}}"**

Ex: {Application ="ApplicationWeb"} | line_format "{{.Path}}"

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/line_formatexample1.PNG?raw=true">
</p>

NOTE: it **doesn't** filter the logs.

### Log pipeline: Label format expressions

The **| label_format** expression can rename, modify or add labels.

Ex: {Path="/ApiTest/BadResult"} | label_format New_Label_Endpoint=Path

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/label_formatexample1.PNG?raw=true">
</p>


# Metric Queries

Metric queries extend log queries by applying a function to log query results.

Loki supports two types of this **range vector** aggregations: 

1 - **Log range aggregations** (range of selected log)

2 - **Unwrapped range aggregations** (range of label values)

It can also use **Built-in Aggregation operators** on top of the metrics queries

## 1 - Log range aggregations

A log range aggregation is a query followed by a duration.

```
rate(log-range): calculates the number of entries per second
count_over_time(log-range): counts the entries for each log stream within the given range.
bytes_rate(log-range): calculates the number of bytes per second for each stream.
bytes_over_time(log-range): counts the amount of bytes used by each log stream for a given range.
absent_over_time(log-range): returns an empty vector if the range vector passed to it has any elements and a 1-element vector with the value 1 if the range vector passed to it has no elements. (absent_over_time is useful for alerting on when no time series and logs stream exist for label combination for a certain amount of time.)
```

**rate(log-range):** 

Ex: rate({Application="ApplicationWeb", StatusCode="400"} [1s])

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/rateexample1.PNG?raw=true">
</p>

Note: time expression [10s] changes the values in the chart.

## 2 - Unwrapped range aggregations.

Unwrapped ranges uses extracted labels as sample values instead of log lines.

However to select which label will be used within the aggregation, the log query must end with an unwrap expression and optionally a label filter expression to discard errors.

The unwrap expression is noted **| unwrap label_identifier** where the label identifier is the label name to use for extracting sample values.

```
rate(unwrapped-range): calculates per second rate of all values in the specified interval.
sum_over_time(unwrapped-range): the sum of all values in the specified interval.
avg_over_time(unwrapped-range): the average value of all points in the specified interval.
max_over_time(unwrapped-range): the maximum value of all points in the specified interval.
min_over_time(unwrapped-range): the minimum value of all points in the specified interval
first_over_time(unwrapped-range): the first value of all points in the specified interval
last_over_time(unwrapped-range): the last value of all points in the specified interval
stdvar_over_time(unwrapped-range): the population standard variance of the values in the specified interval.
stddev_over_time(unwrapped-range): the population standard deviation of the values in the specified interval.
quantile_over_time(scalar,unwrapped-range): the φ-quantile (0 ≤ φ ≤ 1) of the values in the specified interval.
absent_over_time(unwrapped-range): returns an empty vector if the range vector passed to it has any elements and a 1-element vector with the value 1 if the range vector passed to it has no elements. (absent_over_time is useful for alerting on when no time series and logs stream exist for label combination for a certain amount of time.)
```

**sum_over_time(unwrapped-range):** 

Ex: sum_over_time({Application="ApplicationWeb", StatusCode="400"} | unwrap ElapsedMilliseconds [10s])

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/sum_over_timeexample1.PNG?raw=true">
</p>

Ex: sum_over_time({Application="ApplicationWeb"} | json | SourceContext !="Microsoft.AspNetCore.Hosting.Diagnostics" | unwrap Content_Result [10s])

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/agregateresultexample1.PNG?raw=true">
</p>

## Built-in Aggregation operators

We can use built-in aggregation operators over either log or unwrapped range aggregations.

```
<aggr-op>([parameter,] <vector expression>) [without|by (<label list>)]
```

List of **aggr-op**:
```
sum: Calculate sum over labels
avg: Calculate the average over labels
min: Select minimum over labels
max: Select maximum over labels
stddev: Calculate the population standard deviation over labels
stdvar: Calculate the population standard variance over labels
count: Count number of elements in the vector
topk: Select largest k elements by sample value
bottomk: Select smallest k elements by sample value
```

**[parameter,]** is required when using topk and bottomk:
```
Get the top 10 applications by the highest log throughput during the last five minutes 
topk(10,sum(rate({region="us-east1"}[5m])) by (name))
```

**sum**

Ex: sum(rate({Application="ApplicationWeb", StatusCode="400"} [1s]))

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/sum_rateexample1.PNG?raw=true">
</p>

Ex: sum(sum_over_time({Application="ApplicationWeb", StatusCode="400"} | unwrap ElapsedMilliseconds [10s]))

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/sum_sum_over_timeexample1.PNG?raw=true">
</p>

Ex: sum(sum_over_time({Application="ApplicationWeb"} | json | SourceContext !="Microsoft.AspNetCore.Hosting.Diagnostics" | unwrap Content_Result [10s]))

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/sumofagregateresultexample1.PNG?raw=true">
</p>

Ex: sum(count_over_time({Application="ApplicationWeb"} | json | Context=~"ApiTest.*" [5m]))

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/noby_example1.PNG?raw=true">
</p>

**[without|by (<label list>)]** "by" and "without" are used to group the result

Ex: sum(count_over_time({Application="ApplicationWeb"} | json | Context=~"ApiTest.*" [5m])) by (Context)

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/by_example1.PNG?raw=true">
</p>


___________________________________________


# Dashboard

Click to create a new dashboard
Add an empty panel
Choose Loki as data source
Change the Filter time range
Add the **Metric Query** in the Log Browser
Add Title and Save it

Ex: sum(rate({Application="ApplicationWeb", StatusCode="400"} [1s])) by (Path)

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/dashboardexample1.PNG?raw=true">
</p>

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/dashboardexample2.PNG?raw=true">
</p>


Add a new empty panel
In Transform Tab, choose **Labels to fields**
Change visualization type from **Time Series** to **Stats**

<p align="center">
  <img src="https://github.com/RobertoFreireFerrazPassos/Grafana-loki-dotNet-core/blob/main/img/dashboardexample3.PNG?raw=true">
</p>


___________________________________________


# Alert


___________________________________________


# References:

Project setup:

https://medium.com/c-sharp-progarmming/net-core-microservice-logging-with-grafana-and-loki-92cd2783ed88

https://henriquemauri.net/coletando-logs-com-o-serilog-no-net-6/

https://linuxblog.xyz/posts/grafana-loki/

Queries in Loki Grafana:

https://grafana.com/docs/loki/latest/logql/log_queries/

https://grafana.com/docs/loki/latest/logql/metric_queries/

https://megamorf.gitlab.io/cheat-sheets/loki/

https://sbcode.net/grafana/logql/

https://www.youtube.com/watch?v=HDpE9v1Syz8&ab_channel=SBCODE

https://grafana.com/blog/2020/10/28/loki-2.0-released-transform-logs-as-youre-querying-them-and-set-up-alerts-within-loki/

https://grafana.com/blog/2020/08/27/the-concise-guide-to-labels-in-loki/

https://grafana.com/blog/2020/04/21/how-labels-in-loki-can-make-log-queries-faster-and-easier/#what-is-a-label

https://grafana.com/blog/2022/05/12/10-things-you-didnt-know-about-logql/?utm_source=grafana_news&utm_medium=rss


