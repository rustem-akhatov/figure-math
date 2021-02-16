FigureMath
=======
This is an example of ASP.NET 5 application. It allows to keep figure properties in PostgreSQL and calculates their areas.

Supported figures:

* Circle (radius)
* Triangle (height and base)
* Rectangle (length and width)

## Changes to technical requirements

* Path `/figure` replaced by `/figures` 
* `/figures/{id}` returns an entire figure including its area 
* `/figures/{id}/area` returns only area of the figure

## Getting started

FigureMath can be started using Docker Desktop. You can download an  appropriate version on [Docker website](https://www.docker.com/get-started).

To get started:

1) Clone repository

``` console
git clone https://github.com/rustem-akhatov/figure-math.git
```

2) Navigate to cloned directory and run

```console
docker-compose up -d
```

3) Navigate to [home page](http://localhost:9293) (Swagger).

4) To stop services

```console
docker-compose down
```

## Usage examples (Curl)

**Change authority of the URL and identifiers of the figures if needed.**

### Add circle

```console
curl -X POST "http://localhost:9293/figures" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"figureType\":\"Circle\",\"figureProps\":{\"radius\":5.3}}"
```

### Get circle

```console
curl -X GET "http://localhost:9293/figures/1" -H  "accept: */*"
```

### Get area of the circle

```console
curl -X GET "http://localhost:9293/figures/1/area" -H  "accept: */*"
```

### Add triangle

``` console
curl -X POST "http://localhost:9293/figures" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"figureType\":\"Triangle\",\"figureProps\":{\"base\":3.5,\"height\":10.2}}"
```

### Get triangle

```console
curl -X GET "http://localhost:9293/figures/2" -H  "accept: */*"
```

### Get area of the triangle

```console
curl -X GET "http://localhost:9293/figures/2/area" -H  "accept: */*"
```

## How to

### 1) How to add a new figure implementation

1) If you can use TDD and add unit-tests before move on do it or you can add unit-tests later.
1) Add a new name constant to `FigureType` enum.
1) Add class derived from `FigureInfo`. Decorate it with `FigureImplementationAttribute` with the recently added `FigureType` name.
   Implements necessary properties. The class must have at least one public constructor with one parameter of the type `Figure`.
1) Add class implements `IFigureDescriptor` for this figure. It will then be used to validate input parameters for this figure.

That's all. A new figure is now supported.

## TODO

- [ ] Add validation for MediatR requests.
- [ ] Add unit-tests on Common libraries and classes using reflection.
- [ ] Create DSL to describe figures instead of code creation.
