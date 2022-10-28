import pandas as pd
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

if __name__ == '__main__':
    path_to_data = os.getcwd() + '\RowData' 
    Whole_data = make_one_dataset(path_to_data)

