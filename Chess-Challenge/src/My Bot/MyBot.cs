using System;
using ChessChallenge.API;
using Microsoft.CodeAnalysis.FlowAnalysis;

public class MyBot : IChessBot
{
    const int pawnVal = 10;
    const int knightVal = 30;
    const int bishopVal = 30;
    const int rookVal = 50;
    const int queenVal = 90;
    const int kingVal = 900;

    // white = 1, black = -1

    public Move Think(Board board, Timer timer)
    {
        Move[] moves = board.GetLegalMoves();
        int bestScore = int.MinValue;
        Move bestMove = moves[0]; // random default move
        int depth = 3;
        foreach (Move move in moves)
        {
            board.MakeMove(move);
            int score = Minimax(board, depth, false, int.MinValue, int.MaxValue);

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
            board.UndoMove(move);
        }

        return bestMove;
    }


    public int Minimax(Board board, int depth, bool maximizingPlayer, int alpha, int beta)
    {
        if (depth == 0) return Evaluate(board);

        // true white, false black?
        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            Move[] moves = board.GetLegalMoves();

            foreach (var move in moves)
            {
                // Simulate the move
                board.MakeMove(move);
                int eval = Minimax(board, depth - 1, false, alpha, beta);
                // Undo
                board.UndoMove(move);

                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                    break; // beta pruning
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            Move[] moves = board.GetLegalMoves();

            foreach (var move in moves)
            {
                board.MakeMove(move);
                int eval = Minimax(board, depth - 1, true, alpha, beta);
                board.UndoMove(move);
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha) break; // alpha pruning
            }
            return minEval;
        }
    }

    public int Evaluate(Board board)
    {
        int whiteVal = 0;
        int blackVal = 0;
        // get all white black pieces and Count the values
        whiteVal += board.GetPieceList(PieceType.Pawn, true).Count * pawnVal;
        whiteVal += board.GetPieceList(PieceType.Knight, true).Count * knightVal;
        whiteVal += board.GetPieceList(PieceType.Bishop, true).Count * bishopVal;
        whiteVal += board.GetPieceList(PieceType.Rook, true).Count * rookVal;
        whiteVal += board.GetPieceList(PieceType.Queen, true).Count * queenVal;
        whiteVal += board.GetPieceList(PieceType.King, true).Count * kingVal;


        blackVal += board.GetPieceList(PieceType.Pawn, false).Count * pawnVal;
        blackVal += board.GetPieceList(PieceType.Knight, false).Count * knightVal;
        blackVal += board.GetPieceList(PieceType.Bishop, false).Count * bishopVal;
        blackVal += board.GetPieceList(PieceType.Rook, false).Count * rookVal;
        blackVal += board.GetPieceList(PieceType.Queen, false).Count * queenVal;
        blackVal += board.GetPieceList(PieceType.King, false).Count * kingVal;

        int perspective = (board.IsWhiteToMove) ? 1 : -1;

        return (whiteVal - blackVal) * perspective;
    }

}