import pandas as pd
import tqdm
import os

def make_one_dataset(path_to_data: str) -> pd.DataFrame:
    whole_data = pd.DataFrame()
    regions_dirs = os.listdir(path_to_data)
    for region in regions_dirs:
        path_to_one_region = f'{path_to_data}\{region}\DATTSVT.csv'
        current_csv = pd.read_csv(path_to_one_region, encoding='utf-8', delimiter='\t', 
        dtype={'napr': 'str',
               'period': 'str', 
               'nastranapr': 'str', 
               'tnved': 'str', #int64 
               'edizm': 'str', #int64
               'edizm': 'str', #float64
               'Netto': 'str', #float64
               'Kol': 'str',     #int64
               'Region': 'str',
               'Region_s': 'str'
                }) 
        whole_data = pd.concat([whole_data, current_csv])
    return whole_data

path_to_data = os.getcwd() + '\RowData'
whole_data = make_one_dataset(path_to_data)
whole_data['tnved_6'] = 0
for sample in tqdm(range(whole_data.shape[0])):
    whole_data.iloc[[sample]]['tnved_6'] = str(whole_data.iloc[[sample]]['tnved'])[:6]
whole_data.to_csv('whole_data.csv', index = False)