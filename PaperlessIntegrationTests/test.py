import requests, json

IIS_BASE_URL = 'http://localhost:50352/api/'
CONSOLEAPP_BASE_URL = 'http://localhost:5000/api/'
BASE_URL = CONSOLEAPP_BASE_URL

print('using base url: ', BASE_URL)

def test_five_correspondents():
    response = requests.get(BASE_URL + 'correspondents')

    assert response.status_code == 200

    response_data = response.json()

    assert len(response_data) == 5

    print('test_five_correspondents passed')

def test_create_correspondent():
    new_correspondent = {
        'name': 'created_cor',
        'matching_algorithm': 123,
        'match': 'string',
        'is_insensitive': False,
        'owner': 0
    }
    
    response = requests.post(BASE_URL + 'correspondents', json=new_correspondent)

    assert response.status_code == 200

    assert response.text == '6'

    print('test_create_correspondent passed')

def test_delete_correspondent():
    response = requests.delete(BASE_URL + 'correspondents/3')

    assert response.status_code == 200

    print('test_delete_correspondent passed')

def test_create_tag():
    new_tag = {
        "name": "created tag",
        "color": "created tag color",
        "is_inbox_tag": True,
        "matching_algorithm": 0,
        "match": "created tag match",
        "is_insensitive": True,
        "owner": 0
    }

    response = requests.post(BASE_URL + 'tags', json=new_tag)
    
    assert response.status_code == 200
    
    assert response.text == '1'
    
    print('test_create_tag passed')

def test_tag_amount(amount: int):
    response = requests.get(BASE_URL + 'tags')

    assert response.status_code == 200

    assert len(response.json()) == amount

    print('test_tag_amount (', amount, ') passed')

def test_delete_tag():
    response = requests.delete(BASE_URL + 'tags/1')

    assert response.status_code == 200

    print('test_delete_tag passed')

def test_create_document():
    response = requests.post(BASE_URL + 'documents', 
        files = {
            'file1': ('filename1.txt', open('HelloWorld.pdf', 'rb'))
        },
        data={
            "correspondent": 1,
            "document_type": 1,
            "title": "TestTitle",
            "created": "2002-03-20T09:12:28Z",
            "tags": [1, 2, 3]
        })

    assert response.status_code == 200

    print('test_create_document passed')

def test_document_amount(amount: int):
    #response = requests.get(BASE_URL + 'documents')

    print('test_document_amount (', amount, ') passed')


def test_create_document_types():    
    sample_document_type_1 = {
        "name": "TestType1",
        "match": "TestMatch0",
        "matching_algorithm": 200
    }

    sample_document_type_2 = {
        "name": "TestType2",
        "match": "TestMatch2",
        "matching_algorithm": 4400,
        "document_count": 0
    }

    response = requests.post(BASE_URL + 'document_types', json=sample_document_type_1)

    assert response.status_code == 200

    response = requests.post(BASE_URL + 'document_types', json=sample_document_type_2)

    assert response.status_code == 200

    print('test_create_document_types passed')

def test_document_type_amount(amount: int):
    response = requests.get(BASE_URL + 'document_types')

    assert response.status_code == (204 if amount == 0 else 200)

    if response.status_code != 204:
        assert len(response.json()) == amount

    print('test_document_amount (', amount, ') passed')


if __name__ == '__main__':
    test_five_correspondents()
    test_create_correspondent()
    test_delete_correspondent()
    test_five_correspondents()
    test_create_tag()
    test_tag_amount(1)
    test_delete_tag()
    test_tag_amount(0)
    test_create_document()
    test_document_type_amount(0)
    test_create_document_types()
    test_document_type_amount(2)
