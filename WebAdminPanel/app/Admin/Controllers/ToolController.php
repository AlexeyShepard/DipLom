<?php

namespace App\Admin\Controllers;

use Encore\Admin\Controllers\AdminController;
use Encore\Admin\Layout\Content;
use Encore\Admin\Form;
use Encore\Admin\Grid;
use Encore\Admin\Show;
use GuzzleHttp\Client;

class ToolController extends AdminController
{
    public function index(Content $content)
    {
        $generatePinCodeUrl = url('admin/tools/generate-pincode');
        
        return $content
            ->title(trans('base.Tools'))
            ->body(view('tools', ['generatePinCodeUrl' => $generatePinCodeUrl,
                                  'launch' => trans('base.Launch'),                                         
                                  'pinCodeGeneration' => trans('base.PinCodeGeneration')]));
    } 

    public function generatePinCode(Content $content)
    {
        try
        {
            $client = new Client(['base_uri' => 'http://lomapi.isp.regruhosting.ru/api/']);

            $PeoplesJson = $client->request('GET', 'people', [
                'headers' => [
                    'Authorization' => 'Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIyIiwianRpIjoiNGMwZGRkYjI3OGNhODIyMzdlYjFkMjc2M2EyMmNmYzZjZmJkOWVmYzBmNzQwMGUwYzdiMmExMzcwODI5MGEwN2Y1MjY0ZGExMzI5M2YxMWIiLCJpYXQiOjE1ODg2MDI3NDEsIm5iZiI6MTU4ODYwMjc0MSwiZXhwIjoxNjIwMTM4NzQxLCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.V87VBpYbpE4uT5fdWhIiaOSCieFy5vdnFy4oMPkYI3jEL5SA1kR2Z46ZQVc93gYr0c0rl37y4LnOLuARp64zRSF7Z_w099gNZNNVJQYPyLt1o7FawgCH74nopY2-EFUXqOkUfX7yZOVgA34awrNEXNPqWc-pLgn_7XxCy0ySO-iQrXld5Nz1yOb3pagpVSebsML25l9hhahzc7fflo66J2bCwH-FQXSbDMJ0VqOK51OlCpagYK0X3ZsT_8BzaI6UdIgDoFtAQIdSsjqdcEddjwe_AI-ofK3oTwzKmLhyu63vxS8uvEjjcduprAMxp0diUP3V5QSNYHbfYnF66e-_GzSsXJx7TH7hzE0QSimQpDp9CCmAFktbRx36EnHBv3wTNhjV3athJ4DlRU6JfwfKPjagpfDK9IIkX98AGN4ScKO6fe5QR-tydns9kjtcreb4lti5l8jF6yMvbzpVoZQ23w7v-R1V9XD92BoSE2i9hynSRnlAW2wWNI3cZIZbucRcLhbE5p9Aqa0Vs3CDBt-7jDOebPlbvZvUjkHvmEwL1P9WFEhgJLiafZS4036nP6GEus6nNrBjQMSHNK_K_f8nlnm4CqexJynLaORhcZsWvBl1VaH5V0SMPLA2dp2M-vZkrvAWgpwt9-H-V8OeJ2g-4io7EG6O36eaKMApKfJwV-g'   
                ],   
            ]);

            $Peoples = json_decode($PeoplesJson->getBody());
            
            foreach($Peoples as $People)
            {
                $body = "id_People=". $People->id ."&id_EventType=110&HumanOrder=%u041D%u0435%u0442";       

                $responce = $client->request('POST', 'events-handling', [
                    'headers' => [
                        'Content-Type' => 'application/x-www-form-urlencoded',
                        'Authorization' => 'Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiIyIiwianRpIjoiNGMwZGRkYjI3OGNhODIyMzdlYjFkMjc2M2EyMmNmYzZjZmJkOWVmYzBmNzQwMGUwYzdiMmExMzcwODI5MGEwN2Y1MjY0ZGExMzI5M2YxMWIiLCJpYXQiOjE1ODg2MDI3NDEsIm5iZiI6MTU4ODYwMjc0MSwiZXhwIjoxNjIwMTM4NzQxLCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.V87VBpYbpE4uT5fdWhIiaOSCieFy5vdnFy4oMPkYI3jEL5SA1kR2Z46ZQVc93gYr0c0rl37y4LnOLuARp64zRSF7Z_w099gNZNNVJQYPyLt1o7FawgCH74nopY2-EFUXqOkUfX7yZOVgA34awrNEXNPqWc-pLgn_7XxCy0ySO-iQrXld5Nz1yOb3pagpVSebsML25l9hhahzc7fflo66J2bCwH-FQXSbDMJ0VqOK51OlCpagYK0X3ZsT_8BzaI6UdIgDoFtAQIdSsjqdcEddjwe_AI-ofK3oTwzKmLhyu63vxS8uvEjjcduprAMxp0diUP3V5QSNYHbfYnF66e-_GzSsXJx7TH7hzE0QSimQpDp9CCmAFktbRx36EnHBv3wTNhjV3athJ4DlRU6JfwfKPjagpfDK9IIkX98AGN4ScKO6fe5QR-tydns9kjtcreb4lti5l8jF6yMvbzpVoZQ23w7v-R1V9XD92BoSE2i9hynSRnlAW2wWNI3cZIZbucRcLhbE5p9Aqa0Vs3CDBt-7jDOebPlbvZvUjkHvmEwL1P9WFEhgJLiafZS4036nP6GEus6nNrBjQMSHNK_K_f8nlnm4CqexJynLaORhcZsWvBl1VaH5V0SMPLA2dp2M-vZkrvAWgpwt9-H-V8OeJ2g-4io7EG6O36eaKMApKfJwV-g'
                    ],
                    'body' => $body
                ]);
            }

            admin_success('Успех!', 'Пин-коды были сгенерированы успешно!');
        }
        catch (Exception $e)
        {
            admin_error('Провал!', 'Что-то пошло не так =( '. $e->getMessage());
        }
        

        return redirect(url('admin/tools'));
    }
}
