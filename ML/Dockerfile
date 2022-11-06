FROM python:3.9

WORKDIR /ML
COPY requirements.txt /ML
RUN pip install --upgrade pip &&  python -m pip install -r requirements.txt
RUN uvicorn main:app --reload

EXPOSE 8000

COPY . /ML
