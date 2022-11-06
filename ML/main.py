import json
import os

import ranking_modelV2
import pandas as pd
import uuid

from fastapi import FastAPI, File, UploadFile

app = FastAPI()


@app.post("/uploadfile/")
async def create_upload_file(file: UploadFile, periods: list):
    contents = await file.read()
    print(periods)
    id = uuid.uuid4()
    aa = contents.decode('UTF-8')
    f = open(f"{id}.csv", "w")
    f.write(aa)
    f.close()
    whole_data = pd.read_csv(f"{id}.csv", decimal = ',', low_memory=False, delimiter="\t")
    ee = ranking_modelV2.recommendation(whole_data, periods)
    top_10 = 0
    dict = {}
    for tnved in ee.keys():
        if top_10 != 50:
            dict.update({tnved: ee[tnved]})
            top_10+=1
        else:
            break
    os.remove(f"{id}.csv")
    return json.dumps(dict)
