# DummyDataSenderToWebShop

This project generate smartphones data based on images and send to [WebShop](https://github.com/burryfun/WebShop)

Images was scraped using [TechnodomParser](https://github.com/burryfun/TechnodomParser)

## Usage

### Build an image from a Dockerfile

```bash
docker build -t dummy_sender .
```

### Run container

```bash
docker run -it --rm --net=host dummy_sender
```