FROM python:3.9

WORKDIR /ML
COPY requirements.txt /ML
RUN pip install --upgrade pip &&  python -m pip install -r requirements.txt

EXPOSE 5000

COPY . /ML
