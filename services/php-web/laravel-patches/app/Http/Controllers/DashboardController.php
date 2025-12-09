// app/Http/Controllers/DashboardController.php (обновлённый)
<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use App\ViewModels\DashboardViewModel;
use App\Services\JwstService;

class DashboardController extends Controller
{
    private DashboardViewModel $viewModel;
    private JwstService $jwstService;
    
    public function __construct(DashboardViewModel $viewModel, JwstService $jwstService)
    {
        $this->viewModel = $viewModel;
        $this->jwstService = $jwstService;
    }
    
    public function index()
    {
        return view('dashboard', [
            'dashboard' => $this->viewModel->getDashboardData(),
            'metrics' => [
                'iss_speed' => null,
                'iss_alt' => null,
                'neo_total' => 0,
            ],
        ]);
    }
    
    /**
     * /api/jwst/feed — серверный прокси/нормализатор JWST картинок.
     */
    public function jwstFeed(Request $request)
    {
        try {
            $filters = [
                'source' => $request->query('source', 'jpg'),
                'suffix' => $request->query('suffix', ''),
                'program' => $request->query('program', ''),
                'instrument' => strtoupper($request->query('instrument', '')),
                'page' => max(1, (int)$request->query('page', 1)),
                'perPage' => max(1, min(60, (int)$request->query('perPage', 24))),
            ];
            
            $result = $this->jwstService->getImages($filters);
            
            return response()->json([
                'ok' => true,
                'data' => [
                    'source' => $result['source'] ?? '',
                    'count' => $result['count'] ?? 0,
                    'items' => $result['items'] ?? [],
                ],
            ]);
        } catch (\Throwable $e) {
            return response()->json([
                'ok' => false,
                'error' => [
                    'code' => 'JWST_FETCH_ERROR',
                    'message' => 'Ошибка получения данных JWST',
                    'trace_id' => \Str::uuid(),
                ],
            ], 200);
        }
    }
}